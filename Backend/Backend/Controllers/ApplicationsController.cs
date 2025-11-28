using Backend.Entities;
using Backend.Enums;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Microsoft.AspNetCore.Components.Web;
namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationService _service;

        public ApplicationsController(ApplicationService service)
        {
            _service = service;
        }

        [HttpGet("GetAllApp")]
        public async Task<ActionResult<List<ApplicationEntity>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("GetApp/{id}")]
        public async Task<ActionResult<ApplicationEntity>> Get(Guid id)
        {
            var app = await _service.FindByIdAsync(id);
            if (app == null)
                return NotFound(new {message="Заявка не найдена"});
            return Ok(new
            {
                ApplicationId=app.ApplicationId,
                Description = app.Description,
                Cost = app.Cost,
                Status=app.Status,
                Categories=app.Categories
            });
        }

        [HttpPost("CreateApp")]
        public async Task<IActionResult> Create(FrontToApp Fapplication)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("UserId")?.Value;
            var roleClaim = User.FindFirst("Role")?.Value;
            var typeClaim = User.FindFirst("UserType")?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest(new { Message = "Некорректный пользователь или токен" });

            if (string.IsNullOrEmpty(roleClaim))
                return BadRequest(new { Message = "Некорректная роль пользователя", RoleFromToken = roleClaim });

            if (string.IsNullOrEmpty(typeClaim))
                return BadRequest(new { Message = "Некорректный тип пользователя", TypeFromToken = typeClaim });

            if (roleClaim != "Admin" && !(typeClaim == "Advertiser" || typeClaim == "Both"))
                return StatusCode(403, "У вас нет прав на создание заявки");

            if (Fapplication.Cost <= 0)
                return BadRequest(new { Message = "Сумма заявки должна быть положительной" });

            if (!Enum.IsDefined(typeof(ApplicationStatus), Fapplication.Status))
                return BadRequest(new { Message = "Статус заявки некорректен. Допустимые значения: 0, 1, 2" });

            Fapplication.UserId = userId;

            if (Fapplication.Status == 0)
                Fapplication.Status = ApplicationStatus.InProgress;

            ApplicationEntity application = new ApplicationEntity(Fapplication);
            var created = await _service.AddAsync(application);
            return Ok(new
            {   
                ApplicationId=application.ApplicationId,
                Description = application.Description,
                Cost = application.Cost,
                Status=application.Status,
                Categories=application.Categories
            }) ;          
            
            
        }

        [HttpPut("UpdataApp/{id}")]
        public async Task<IActionResult> Update(Guid id, FrontToApp Fupdated)
        {
            var updated = new ApplicationEntity(Fupdated);
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest(new { Message = "Некорректный пользователь" });

            var roleClaim = User.FindFirst("Role")?.Value;
            if (string.IsNullOrEmpty(roleClaim))
                return BadRequest(new { Message = "Некорректная роль пользователя", RoleFromToken = roleClaim });

            if (!Enum.IsDefined(typeof(ApplicationStatus), updated.Status))
                return BadRequest(new { Message = "Статус заявки некорректен." });

            var result = await _service.UpdateAsync(id, updated, userId, roleClaim);
            if (!result)
                return NotFound("Заявка не найдена");

            return NoContent();
        }

        [HttpGet("GetByUser")]
        public async Task<ActionResult<List<ApplicationEntity>>> GetByUser()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null || !Guid.TryParse(userId, out Guid parsedUserId))
                return BadRequest(new { Message = "Некорректный пользователь или токен" });

            var apps = await _service.GetByUserIdAsync(parsedUserId);

            if (apps == null || !apps.Any())
                return NotFound("Заявки пользователя не найдены");

            return Ok(apps.Select(app => new
            {   ApplicationId=app.ApplicationId,
                Description = app.Description,
                Cost = app.Cost,
                Status=app.Status,
                Categories=app.Categories
                
            }));
        }


        [HttpDelete("DeleteApp/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest(new { Message = "Некорректный пользователь" });

            var roleClaim = User.FindFirst("Role")?.Value;
            if (string.IsNullOrEmpty(roleClaim))
                return BadRequest(new { Message = "Некорректная роль пользователя", RoleFromToken = roleClaim });

            var result = await _service.DeleteAsync(id, userId, roleClaim);
            if (!result)
                return NotFound("Заявка не найдена или доступ запрещен");
            return NoContent();
        }
    }
}



