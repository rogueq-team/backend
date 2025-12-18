using System.Security.Claims;
using System.Threading.Tasks;
using Backend;
using Backend.DataDto;
using Backend.Entities;
using Backend.Enums;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("[controller]")]
public class DealController : ControllerBase
{
    private readonly DealService _dealService;
    private readonly ApplicationService _applictionService;
    private readonly UserService _userService;

    public DealController(DealService dealService, ApplicationService applicationService, UserService userService)
    {
        this._dealService = dealService;
        this._applictionService = applicationService;
        this._userService = userService;
    }


    [HttpGet("GetByApplication/{applicationId}")]
    [Authorize]
    public async Task<IActionResult> GetDealsByApplication(Guid applicationId)
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (userId is null)
            return BadRequest(new { Message = "Некоректный пользователь" });
        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        if (user is null)
            return NotFound(new { Message = "Пользователь не найден" });

        var deals = await _dealService.FindByApplicationIdAsync(applicationId);

        foreach (DealEntity deal in deals)
        {
            if (!(Guid.Parse(userId) == deal.AdvertiserId && user.Type == UserType.Advertiser || Guid.Parse(userId) == deal.PlatformId && user.Type == UserType.Platform || user.Role == UserRole.Admin))
                deals.Remove(deal);
        }

        return Ok(deals.Select(deal => new
        {
            DealId = deal.DealId,
            ApplicationId = deal.ApplicationId,
            AdvertiserId = deal.AdvertiserId,
            PlatformId = deal.PlatformId,
            Description = deal.Description,
            Status = deal.Status,
            CreatedDate = deal.CreatedAt,
            AdvertiserName = deal.Advertiser?.Name,
            PlatformName = deal.Platform?.Name,
            Advertiser = new
            {
                deal.Advertiser.UserId,
                deal.Advertiser.Name,
                deal.Advertiser.Email
            },
            Platform = new
            {
                deal.Platform.UserId,
                deal.Platform.Name,
                deal.Platform.Email
            }
        }));
    }

    [HttpGet("GetDeal/{dealId}")]
    [Authorize]
    public async Task<IActionResult> GetDeal(Guid dealId)
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (userId is null)
            return BadRequest(new { message = "Некоректный пользователь" });
        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        if (user is null)
            return NotFound(new { message = "Пользователь не найден" });


        var deal = await _dealService.FindByDealIdAsync(dealId);
        if (deal == null)
            return NotFound(new { message = "Сделка не найдена" });
        if (!(Guid.Parse(userId) == deal.AdvertiserId && user.Type == UserType.Advertiser || Guid.Parse(userId) == deal.PlatformId && user.Type == UserType.Platform || user.Role == UserRole.Admin))
            return BadRequest(new { message = "Недостаточно прав доступа" });

        return Ok(new
        {
            DealId = deal.DealId,
            ApplicationId = deal.ApplicationId,
            AdvertiserId = deal.AdvertiserId,
            PlatformId = deal.PlatformId,
            Description = deal.Description,
            Status = deal.Status,
            CreatedDate = deal.CreatedAt,
            Advertiser = new
            {
                deal.Advertiser.UserId,
                deal.Advertiser.Name,
                deal.Advertiser.Email
            },
            Platform = new
            {
                deal.Platform.UserId,
                deal.Platform.Name,
                deal.Platform.Email
            }
        });
    }

    [HttpPost("ChangeStatus")]
    [Authorize]
    public async Task<IActionResult> ChangeStatus(Guid dealId, string status)
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (userId is null)
            return BadRequest(new { message = "Некорректный пользователь" });
        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        if (user is null)
            return NotFound(new { message = "Пользователь не найден" });
        var deal = await _dealService.FindByDealIdAsync(dealId);
        if (deal == null)
            return BadRequest(new { message = "Сделки не существует или данный пользователь не имеет к ней доступа" });
        if (!(deal.ApplicationId == user.UserId || deal.PlatformId == user.UserId))
            return BadRequest(new { message = "Сделки не существует или данный пользователь не имеет к ней доступа" });

        if (false == await _dealService.ChangeStatusById(dealId, status))
            return BadRequest(new { message = "Невозможный статус" });
        return Ok(new
        {
            dealID = deal.DealId,
            status = $"{deal.Status}"
        });

    }


    [HttpPost("ChangeDescription")]
    [Authorize]
    public async Task<IActionResult> ChangeDescription(Guid dealId, string description)
    {
        var deal = await _dealService.FindByDealIdAsync(dealId);
        if (deal == null)
            return BadRequest(new { Message = "Заявки не существует" });
        var userId = User.FindFirst("UserId")?.Value;
        if (userId == null)
            return BadRequest(new { Message = "Некорректный пользователь" });
        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        if (user is null)
            return NotFound(new { Message = "Пользователь не найден" });
        if (deal.AdvertiserId != user.UserId && deal.PlatformId != user.UserId)
            return BadRequest(new { Message = "Нет доступа к сделке" });
        deal.Description = description;
        if ((await _dealService.UpdateDealAsync(deal)) == false)
            return BadRequest(new { Messge = "Ошибка обновления описания" });
        return Ok(new
        {
            DealId = deal.DealId,
            ApplicationId = deal.ApplicationId,
            AdvertiserId = deal.AdvertiserId,
            PlatformId = deal.PlatformId,
            Description = deal.Description,
            Status = deal.Status,
            CreatedDate = deal.CreatedAt,
            Advertiser = new
            {
                deal.Advertiser.UserId,
                deal.Advertiser.Name,
                deal.Advertiser.Email
            },
            Platform = new
            {
                deal.Platform.UserId,
                deal.Platform.Name,
                deal.Platform.Email
            }
        });

    }

    [HttpPost("CreateDeal")]
    [AuthorizeAttribute(roles: new[] { "Admin", "User" }, types: new[] { "Platform", "both" })]
    public async Task<IActionResult> CreateDeal(Guid applicationId, string description)
    {
        var application = await _applictionService.FindByIdAsync(applicationId);
        if (application == null)
            return NotFound(new { message = "Заявка с таким Id не найдена" });
        var advertiser = await _userService.FindByIdAsync(application.UserId);
        if (advertiser == null)
            return NotFound(new { message = "Пользователь не найден" });
        if (advertiser.Type != UserType.Advertiser)
            return NotFound(new { message = "В заявке указан неккоректный пользователь" });
        var platformId = User.FindFirst("UserId")?.Value;
        if (platformId is null)
            return BadRequest(new { message = "Некоректный пользователь" });
        var platform = await _userService.FindByIdAsync(Guid.Parse(platformId));
        if (platform is null)
            return NotFound(new { message = "Пользователь не найден" });
        if (platform.UserId == advertiser.UserId)
            return BadRequest(new { message = "Невозможно принять свою же заявку" });

        if (platform.Balance<application.Cost)
            return BadRequest(new {message="Недостаточно средств"});
        platform.Balance-=application.Cost;
        DealEntity newDeal = new DealEntity { ApplicationId = applicationId, AdvertiserId = advertiser.UserId, PlatformId = Guid.Parse(platformId), Description = description, Advertiser = advertiser, Platform = platform, Status = DealStatus.InProgress };
        if (application.Deals == null)
        {
            application.Deals = new List<DealEntity>();
        }
        application.Deals.Add(newDeal);

        
        bool flag = await _dealService.AddAsync(newDeal);
        if (flag is true)
            
            return Ok(new
            {
                DealId = newDeal.DealId,
                ApplicationId = newDeal.ApplicationId,
                AdvertiserId = newDeal.AdvertiserId,
                PlatformId = newDeal.PlatformId,
                Description = newDeal.Description,
                Status = newDeal.Status,
                CreatedDate = newDeal.CreatedAt,
                Advertiser = new
                {
                    newDeal.Advertiser.UserId,
                    newDeal.Advertiser.Name,
                    newDeal.Advertiser.Email
                },
                Platform = new
                {
                    newDeal.Platform.UserId,
                    newDeal.Platform.Name,
                    newDeal.Platform.Email
                }
            });

        else return BadRequest(new { message = "Не удалось создать сделку" });
    }

}


