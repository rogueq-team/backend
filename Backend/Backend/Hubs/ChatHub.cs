using System.Threading.Tasks;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Enums;
using Microsoft.AspNetCore.Http; 
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Backend.Services;
using Microsoft.IdentityModel.Tokens;
namespace Backend.Hubs
{
    



[Authorize]
public class ChatHub: Hub
{
    private readonly MessageService _messageService;
    private readonly UserService _userService;
    private readonly DealService _dealService;
    
    public ChatHub(MessageService ms, UserService us, DealService ds )
    {
        _messageService=ms;
        _userService=us;
        _dealService=ds;      
    }

    [HubMethodName("SendTo")]
    public async Task SendTo(MassegeDto messageDto)
    {
        if ( messageDto.Text.IsNullOrEmpty())
        {
            await Clients.Caller.SendAsync("Error","Не авторизован");
            return ;
        }
        var deal=await _dealService.FindByDealIdAsync(messageDto.DealId);
        if (deal==null)
        {
            await Clients.Caller.SendAsync("Error","Получатель не найден");
            return ;
        }
        var userId =GetUserId();
        if (userId is null)
        {
            await Clients.Caller.SendAsync("Error","Не авторизован");
            return ;
        }
        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        if (user is null)
        {
            await Clients.Caller.SendAsync("Error","Не авторизован");
            return ;
        }
                if (deal == null || (deal.AdvertiserId.ToString() != userId && deal.PlatformId.ToString() != userId))
        {
            await Clients.Caller.SendAsync("Error", "Доступ запрещен");
            return;
        }
        MessageEntity message=new MessageEntity(messageDto.DealId,deal,user.UserId,user,messageDto.Text);
      
        Guid UserToId;
        if (message.UserId==message.Deal?.AdvertiserId)
        {
                UserToId=message.Deal.PlatformId;
        }
        else if (message.UserId==message.Deal?.PlatformId)
        {
            UserToId=message.Deal.AdvertiserId;
        } else
            {
                await Clients.Caller.SendAsync("Error","Получатель не найден");
                return ;
            }
         if (!await _messageService.AddAsync(message))
            {
                await Clients.Caller.SendAsync("Error","Ошибка отправки сообщения, пропробуйте ещё раз");
                return;
            }
        await Clients.User(UserToId.ToString()).SendAsync("GetMessage",message.Text);
        return ;

    }

    [HubMethodName("GetMessageHistory")]
    public async Task GetMessageHistory(Guid dealId, int page = 1, int pageSize = 50)
    {
    try
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            await Clients.Caller.SendAsync("Error", "Не авторизован");
            return;
        }

    
        var deal = await _dealService.FindByDealIdAsync(dealId);
        if (deal == null || (deal.AdvertiserId.ToString() != userId && deal.PlatformId.ToString() != userId))
        {
            await Clients.Caller.SendAsync("Error", "Доступ запрещен");
            return;
        }

        var messages = await _messageService.GetMessagesByDealIdAsync(dealId, page, pageSize);
        var userGuid = Guid.Parse(userId);

        var response = messages.Select(m => new
        {
            MessageId = m.Id,
            Text = m.Text,
            SenderId = m.UserId,
            SenderName = m.User?.Name,
            DealId = m.DealId,
            Timestamp = m.CreatedAt,
            IsOwn = m.UserId == userGuid,
        }).ToList();

        await Clients.Caller.SendAsync("MessageHistory", new
        {
            Messages = response,
            TotalCount = await _messageService.GetMessagesCountAsync(dealId),
            Page = page,
            PageSize = pageSize
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error getting history: {ex}");
        await Clients.Caller.SendAsync("Error", "Ошибка получения истории");
    }
}

    private string? GetUserId()
    {
           
            return  Context.User?.FindFirst("UserId")?.Value;
     }
}

public class CustomUserIdProvider: IUserIdProvider
{
    public virtual string? GetUserId(HubConnectionContext connection)
    {
         return connection.User?.FindFirst("UserId")?.Value;
    }
}
}