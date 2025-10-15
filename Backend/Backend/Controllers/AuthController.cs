using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
     private readonly IConfiguration _configuration;
    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var user = UserService.FindById(id);
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
    [HttpPost("registration")]
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
        return CreatedAtAction(nameof(Get), new { id = User.Id }, user);
    }

    [HttpPost("Authentication")]
    public IActionResult Authentication(AuthUser request)
    {
        var user = UserService.FindByEmail(request.LoginOrEmail) ?? UserService.FindByLogin(request.LoginOrEmail);
        if (user is null)
            return Unauthorized(new { message = "Неверный логин/Email или пароль" });
        if (!PasswordService.VerifyPassword(request.password, user.Password))
             return Unauthorized(new { message = "Неверный логин/Email или пароль" });

        var JWTServic = new JWTService(_configuration);
        string token = JWTServic.GenerateToken(user);
        return Ok(new
        {
            token = token,
            user = new
            {
                id = user.Id,
                login = user.Login,
                email = user.Email,
                role = user.Role
            }
        });
            
            
        
    }

}


