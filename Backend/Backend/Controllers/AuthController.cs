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

    public AuthController(IConfiguration configuration, RefreshTokenService refreshTokenService,JWTService jwtService)
    {
        _configuration = configuration;
        this.refreshTokenService = refreshTokenService;
        this.jwtService = jwtService;
    }
    [HttpGet("{Id}")]

    //Нужно подумать, чтобы чел могу получать инфу о себе и тд
    public IActionResult Get(int Id)
    {
        var user = UserService.FindById(Id);
        if (user is null)
            return NotFound();
        RegUser User = new(user);
        return Ok(User);
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(UserService.GetAllReg());
    }
    [HttpPost("Registration")]
    public IActionResult Registration(User User)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        if (!(UserService.FindByEmail(User.Email) is null))
            return Conflict(new { message = "Пользователь с таким email уже существует" });
        if (!(UserService.FindByLogin(User.Login) is null))
            return Conflict(new { message = "Пользователь с таким логином уже существует" });
        UserService.Add(User);
        RegUser user = new(User);
        string JWTToken = jwtService.GenerateAccesToken(User);
        string RefreshToken = refreshTokenService.CreateRefreshToken(User.Id);
        return Ok(new
        {
            ///посмотреть ролевую модель айдентити сервер
            Login = User.Login,
            Email = User.Email,
            Role = User.Role,
            UserType = User.UserType,
            JWTToken = JWTToken,
            RefreshToken = RefreshToken
        });

    }

    [HttpPost("Authentication")]
    public IActionResult Authentication(AuthUser request)
    {
        var user = UserService.FindByEmail(request.LoginOrEmail) ?? UserService.FindByLogin(request.LoginOrEmail);
        if (user is null)
            return Unauthorized(new { message = "Неверный логин/Email или пароль" });
        if (!PasswordService.VerifyPassword(request.password, user.Password))
            return Unauthorized(new { message = "Неверный логин/Email или пароль" });

        string JWTtoken = jwtService.GenerateAccesToken(user);
        string RefreshToken = refreshTokenService.CreateRefreshToken(user.Id);

        return Ok(new
        {
            JWTtoken = JWTtoken,
            RefreshToken = RefreshToken,
            user = new
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                Role = user.Role
            }
        });

    }

    // [HttpGet("RefreshToken/Getall")]
    // public IActionResult GetAllTokens()
    // {
    //     return Ok(refreshTokenService.GetAll());
    // }
    public class RefreshDto         //Вынести
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    //норм
    [HttpPost("RefreshToken")]
    public IActionResult RefreshToken(RefreshDto refreshToken )
    {
        if (string.IsNullOrEmpty(refreshToken.RefreshToken))
            return Unauthorized(new { message = "RefreshToken отсутствует" });


        System.Console.WriteLine($"AAA{refreshToken.RefreshToken}");
        var Token = refreshTokenService.GetRefreshTokenByToken(refreshToken.RefreshToken);
        System.Console.WriteLine(Token);
        if (Token is null || !Token.IsActive)
            return Unauthorized(new { message = "Истекший RefreshToken" });
     
        var user = UserService.FindById(Token.UserId);
        if (user is null)
            return Unauthorized(new { message = "Пользователь не найден" });

        
        string NewJWTToken = jwtService.GenerateAccesToken(user);
        string RefreshToken = refreshTokenService.CreateRefreshToken(user.Id);

        return Ok(new
        {
            JwtToken = NewJWTToken,
            RefreshToken = RefreshToken
        });
    }

    [HttpDelete("Delete")]
    public IActionResult Delete(string email)
    {
        User? userBd = UserService.FindByEmail(email);
        if (userBd is null)
            return NotFound(new { message = "Такого пользователя не существует" });
        UserService.Delete(userBd.Id);
        return Ok(new
        {
            user = new
            {
                Id = userBd.Id,
                Login = userBd.Login,
                Email = userBd.Email,
                Role = userBd.Role
            }
        });
    }

    //ВСЁ ПЕРЕДЕЛАТЬ
    [HttpPut("Deactivate/{Id}")]
    public IActionResult Deactivate(int Id)
    {
        var user = UserService.FindById(Id);
        if (user is null)
            return NotFound();
        else
        {
            if (user.DeletedAt == (new DateTime()))
            {
                user.DeletedAt = DateTime.Now;
                return Ok(new
                {
                    Id = user.Id,
                    DeletedAt = user.DeletedAt
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Пользователь является деактеваированным"
                });
            }

        }
    }
    [HttpPut("Activate{Id}")]
    public IActionResult Activate(int Id)
    {
        var user = UserService.FindById(Id);
        if (user is null)
            return NotFound();
        else
        {
            if (user.DeletedAt == (new DateTime()))
            {
                return BadRequest(new
                {
                    message = "Пользователь является активированным"
                });
                
            }
            else
            {
                user.DeletedAt = new DateTime();
                return Ok(new
                {
                    Id = user.Id,
                    DeletedAt = user.DeletedAt
                });
            }
        }
    }
}


