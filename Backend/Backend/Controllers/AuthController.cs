using System.Security.Claims;
using System.Threading.Tasks;
using Backend;
using Backend.Entities;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly RefreshTokenService refreshTokenService;
    private readonly JWTService jwtService;
    private readonly UserService _userService;

    public AuthController(IConfiguration configuration, RefreshTokenService refreshTokenService, JWTService jwtService, UserService userService)
    {
        _configuration = configuration;
        this.refreshTokenService = refreshTokenService;
        this.jwtService = jwtService;
        this._userService = userService;
    }

    [HttpGet("Me")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (userId is null)
            return BadRequest(new { Message = "UserId is not found in token" });
        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        if (user is null)
            return NotFound(new { Message = "User not found" });
        return Ok(new UserToFront(user));

    }
    [HttpGet("Check-headers")]
    public IActionResult CheckHeaders()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();

        return Ok(new
        {
            AuthHeader = authHeader,
            HasAuthHeader = !string.IsNullOrEmpty(authHeader),
            AllHeaders = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
        });
    }

    [HttpPost("Registration")]
    public async Task<IActionResult> Registration(FrontToUser FUser)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        if (!(await _userService.FindByEmailAsync(FUser.Email) is null))
            return Conflict(new { message = "Пользователь с таким email уже существует" });
        if (!(await _userService.FindByLoginAsync(FUser.Login) is null))
            return Conflict(new { message = "Пользователь с таким логином уже существует" });
        UserEntity User = new UserEntity(FUser);
        var result = await _userService.AddAsync(User);
        if (result)
        {

            string JWTToken = jwtService.GenerateAccesToken(User);
            string RefreshToken = await refreshTokenService.CreateRefreshToken(User.UserId);

            return Ok(new
            {
                Login = User.Login,
                Email = User.Email,
                Role = $"{User.Role}",
                UserType = $"{User.Type}",
                JWTToken = JWTToken,
                RefreshToken = RefreshToken
            });
        }
        else
        {
            return Conflict(new { message = "Ошибка при добавлении пользователя" });
        }

    }

    [HttpPost("Authentication")]
    public async Task<IActionResult> Authentication(AuthUser request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _userService.FindByEmailAsync(request.Email);
        if (user is null)
            return Unauthorized(new { message = "Неверный Email или пароль" });
        if (!PasswordService.VerifyPassword(request.password, user.Password))
            return Unauthorized(new { message = "Неверный Email или пароль" });

        string JWTtoken = jwtService.GenerateAccesToken(user);
        string RefreshToken = await refreshTokenService.CreateRefreshToken(user.UserId);

        return Ok(new
        {
            JWTtoken = JWTtoken,
            RefreshToken = RefreshToken,
            user = new
            {
                Id = user.UserId,
                Login = user.Login,
                Email = user.Email,
                Role = $"{user.Role}",
                Type = $"{user.Type}"
            }
        });

    }
    [HttpGet("GetAll")]
    [Authorize("Admin")]
    public async Task<IActionResult> GetAll()
    {
        var listik = await _userService.GetAll();
        return Ok(new { Persons = listik });
    }
    [HttpPost("RefreshToken")]
    [Authorize]
    public async Task<IActionResult> RefreshToken(RefreshDto refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken.RefreshToken))
            return Unauthorized(new { message = "RefreshToken отсутствует" });


        var Token = refreshTokenService.GetRefreshTokenByToken(refreshToken.RefreshToken);
        System.Console.WriteLine(Token);
        if (Token is null || !Token.IsActive)
            return Unauthorized(new { message = "Истекший RefreshToken" });

        var user = await _userService.FindByIdAsync(Token.UserId);
        if (user is null)
            return Unauthorized(new { message = "Пользователь не найден" });


        string NewJWTToken = jwtService.GenerateAccesToken(user);
        string RefreshToken = await refreshTokenService.CreateRefreshToken(user.UserId);

        return Ok(new
        {
            JwtToken = NewJWTToken,
            RefreshToken = RefreshToken
        });
    }

    [HttpDelete("Delete")]
    [Authorize]
    public async Task<IActionResult> Delete()
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var userGuid))
            return BadRequest(new { message = "Невалидный пользовательский ID" });

        var currentUser = await _userService.FindByIdAsync(userGuid);
        if (currentUser is null)
            return NotFound(new { message = "Пользователь не найден" });

        _userService.Delete(userGuid);

        return Ok(new
        {
            user = new { Id = currentUser.UserId, Email = currentUser.Email }
        });
    }


}


