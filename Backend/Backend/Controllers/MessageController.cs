// using System.Security.Claims;
// using System.Threading.Tasks;
// using Backend;
// using Backend.Entities;
// using Backend.Hubs;
// using Backend.Models;
// using Backend.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;


// [Authorize]
// [ApiController]
// [Route("[controller]")]
// public class MessageController : ControllerBase
// {

//     private readonly UserService _userService;
//     private readonly MessageService _messageService;
//     private readonly DealService _dealService;
//     private readonly ChatHub _chatHub;


//     public MessageController(MessageService messageService,ChatHub chatHub,DealService dealService,UserService userService)
//     {
//         _messageService=messageService;
//         _chatHub=chatHub;
//         _dealService=dealService;
//         _userService=userService;

//     }

    
//     [Authorize]
//     [HttpPost("SendMessage")]
//     public async Task<IActionResult> SendMessage(MassegeDto messageDto)
//     {
//         if ( messageDto.Text.IsNullOrEmpty())
//             return BadRequest(new {Message="Ошибка отправки сообщения"});
//         var deal=await _dealService.FindByDealIdAsync(messageDto.DealId);
//         if (deal==null)
//             return BadRequest(new {Message="Ошибка отправки сообщения"});
//         var userId = User.FindFirst("UserId")?.Value;
//         if (userId is null)
//             return BadRequest(new { message = "Некоректный пользователь" });
//         var user = await _userService.FindByIdAsync(Guid.Parse(userId));
//         if (user is null)
//             return NotFound(new { message = "Пользователь не найден" });
//         MessageEntity message=new MessageEntity(messageDto.DealId,deal,user.UserId,user,messageDto.Text);
//         await _messageService.AddAsync(message);
//         bool result = await _chatHub.SendTo(message);
//         if (result)
//             return Ok();
//         else return BadRequest(new {Massege="Ошибка отправки сообщения"});
//     }



// }


