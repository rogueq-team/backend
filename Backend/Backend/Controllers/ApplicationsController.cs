using Backend.Entities;
using Backend.Enums;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<ActionResult<List<AppToFront>>> GetAll()
        {
            List<AppToFront> Res = new List<AppToFront>();

            List<ApplicationEntity> Ents= await _service.GetAllAsync();
            if (!Ents.IsNullOrEmpty())
            {
                foreach (var Ent in Ents)
                    Res.Add(new AppToFront(Ent));
            } else return BadRequest(new {Massage="База данных пуста или произошла ошибка получения данных"});
            return Res;
        }

        [HttpGet("GetApp/{id}")]
        public async Task<ActionResult<AppToFront>> Get(Guid id)
        {
            var app= await _service.FindByIdAsync(id);
            if (app == null)
                return NotFound("Заявка не найдена");
            AppToFront Resapp= new AppToFront(app);
            return Resapp;
        }

        [HttpPost("CreateApp")]
        public async Task<IActionResult> Create(FrontToApp Application)
        {
            ApplicationEntity application = new ApplicationEntity(Application);
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

            if (application.Cost <= 0)
                return BadRequest(new { Message = "Сумма заявки должна быть положительной" });

            if (!Enum.IsDefined(typeof(ApplicationStatus), application.Status))
                return BadRequest(new { Message = "Статус заявки некорректен. Допустимые значения: 0, 1, 2" });

            application.UserId = userId;



            var created = await _service.AddAsync(application);
            return CreatedAtAction(nameof(Get), new { id = created.ApplicationId }, created);
        }

        [HttpPut("ApplicationUpdate/{id}")]
        public async Task<IActionResult> Update(Guid id, FrontToApp Updated)
        {
            ApplicationEntity updated= new ApplicationEntity(Updated);
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
        public async Task<ActionResult<List<AppToFront>>> GetByUser()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null || !Guid.TryParse(userId, out Guid parsedUserId))
                return BadRequest(new { Message = "Некорректный пользователь или токен" });
            
            var apps = await _service.GetByUserIdAsync(parsedUserId);

            if (apps == null || !apps.Any())
                return NotFound("Заявки пользователя не найдены");
            List<AppToFront> Res= new List<AppToFront>();
            foreach (var app in apps)
                    Res.Add(new AppToFront(app));

            return Ok(Res);
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
