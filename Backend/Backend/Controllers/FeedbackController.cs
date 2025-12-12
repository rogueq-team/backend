using Backend.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;
        private readonly ApplicationService _applictionService;
        private readonly UserService _userService;

        public FeedbackController(FeedbackService feedbackService, ApplicationService applicationService, UserService userService)
        {
            this._feedbackService= feedbackService;
            this._applictionService = applicationService;
            this._userService = userService;
        }

        
    } 
}
