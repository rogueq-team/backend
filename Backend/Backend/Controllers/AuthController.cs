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
        return Ok(user);
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(UserService.GetAll());
    }
    [HttpPost("registration")]
    public IActionResult Registration(User user)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        if (!(UserService.FindByEmail(user.Email) is null))
            return Conflict(new { message = "Пользователь с таким email уже существует" });
        if (!(UserService.FindByLogin(user.Login) is null))
            return Conflict(new { message = "Пользователь с таким логином уже существует" });
        UserService.Add(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPost("Authentication")]
    public IActionResult Authentication(AuthRequest request)
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


