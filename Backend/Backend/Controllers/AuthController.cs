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
    [HttpGet("{Id}")]
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
          double time = 1;
        double.TryParse(_configuration["JWTOptions:timeout"], out time);
        Response.Cookies.Append("NoJWT", token, new CookieOptions { Expires = DateTime.UtcNow.AddHours(time) });
        return Ok(new
        {
            token = token,
            user = new
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                Role = user.Role
            }
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

    [HttpPut("Deactivate{Id}")]
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


