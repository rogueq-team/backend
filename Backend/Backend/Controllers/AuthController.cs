using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
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
    public IActionResult Authentication(User user)
    {
        
    }

}


