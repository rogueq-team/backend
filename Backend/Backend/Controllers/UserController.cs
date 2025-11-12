using System.Security.Claims;
using System.Threading.Tasks;
using Backend;
using Backend.Entities;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly RefreshTokenService refreshTokenService;
    private readonly JWTService jwtService;
    private readonly UserService _userService;


    public UserController(IConfiguration configuration, RefreshTokenService refreshTokenService, JWTService jwtService, UserService userService)
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

    [HttpPost("UpdateInformation")]
    [Authorize]
    public async Task<IActionResult> UpdateProdileData(UserControllerDTO UserDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Massage = "Ошибка в отправленных данных" });
        var userId = User.FindFirst("UserId")?.Value;
        if (userId is null)
            return BadRequest(new { Message = "Некорректный пользователь" });
        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        if (user is null)
            return NotFound(new { Message = "Пользователь не найден" });
        var updatedUser = new UserEntity(UserDTO);
        if (!await _userService.UpdateUserAsync(UserDTO, user.UserId))
            return BadRequest(new { Massage = "Ошибка обновления данных" });
        return Ok(new UserToFront(updatedUser));
    }

    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Massage = "Ошибка в отправленных данных" });
        var userId = User.FindFirst("UserId")?.Value;
        if (userId is null)
            return BadRequest(new { Message = "Некорректный пользователь" });
        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        if (user is null)
            return NotFound(new { Message = "Пользователь не найден" });
        if (!PasswordService.VerifyPassword(oldPassword, user.Password))
            return Unauthorized(new { message = "Неверный  пароль" });
        user.Password = PasswordService.HashPassword(newPassword);
        if (!await _userService.UpdateUserAsync(user))
            return BadRequest(new { Massage = "Ошибка обновления данных" });
        return Ok(new {Message="Пароль обновлён"});
    }

    

    

   
   

    

}


