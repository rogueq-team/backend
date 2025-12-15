using Backend.DataTransfer;
using Backend.Enums;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;
        private readonly ApplicationService _applicationService;
        private readonly UserService _userService;

        public FeedbackController(FeedbackService feedbackService, ApplicationService applicationService, UserService userService)
        {
            this._feedbackService = feedbackService;
            this._applicationService = applicationService;
            this._userService = userService;
        }

        [HttpPost("CreatFeed")]
        [Authorize]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Некорректные данные" });

            var userIdStr = User.FindFirst("UserId")?.Value;
            if (userIdStr is null)
                return Unauthorized(new { Message = "UserId не найден в токене" });

            var senderId = Guid.Parse(userIdStr);

            var result = await _feedbackService.CreateFeedbackAsync(
                dto.DealId,
                senderId,
                dto.Stars,
                dto.Text);

            if (!result.Success)
                return BadRequest(new { Message = result.Message });

            return Ok(new { Message = result.Message });
        }


        [HttpGet("GetById/feedId")]
        public async Task<IActionResult> GetFeedback(Guid id)
        {
            var feedback = await _feedbackService.FindByIdAsync(id);

            if (feedback == null)
                return NotFound(new { Message = "Отзыв не найден" });

            return Ok(feedback);
        }


        [HttpGet("GetByUser/userId")]
        public async Task<IActionResult> GetFeedbacksForUser(Guid userId)
        {
            var feedbacks = await _feedbackService.FindByRecipientIdAsync(userId);
            return Ok(feedbacks);
        }


        [HttpDelete("delete/feedId")]
        [Authorize]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (userIdStr is null)
                return Unauthorized(new { Message = "UserId не найден в токене" });

            var user = await _userService.FindByIdAsync(Guid.Parse(userIdStr));
            if (user == null)
                return Unauthorized(new { Message = "Пользователь не найден" });

            var result = await _feedbackService.DeleteFeedbackAsync(id, user.Role);

            if (!result.Success)
                return BadRequest(new { Message = result.Message });

            return Ok(new { Message = result.Message });
        }
    }

}
