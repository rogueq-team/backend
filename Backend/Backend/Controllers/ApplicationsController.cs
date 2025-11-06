using Backend.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationService _service;

        public ApplicationsController(ApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationEntity>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationEntity>> Get(Guid id)
        {
            var app = await _service.FindByIdAsync(id);
            if (app == null)
                return NotFound("Заявка не найдена");
            return app;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationEntity application)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var created = await _service.AddAsync(application);
            return CreatedAtAction(nameof(Get), new { id = created.ApplicationId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ApplicationEntity updated)
        {
            if (id != updated.ApplicationId)
                return BadRequest("ID в запросе и в теле не совпадают");

            var result = await _service.UpdateAsync(updated);
            if (!result)
                return NotFound("Заявка не найдена");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound("Заявка не найдена");

            return NoContent();
        }
    }
}
