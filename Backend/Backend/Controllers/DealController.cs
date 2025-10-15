using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class DealController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public DealController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [HttpGet("Deal{id}")]
    public IActionResult GetDeal(int id)
    {
        var deal = DealService.FindByDealId(id);
        if (deal is null)
            return NotFound();
        return Ok(deal);
    }
    [HttpGet("Application{id}")]
    public IActionResult GetApplication(int id)
    {
        var deal = DealService.FindByApplicationId(id);
        if (deal is null)
            return NotFound();
        return Ok(deal);
    }

    [HttpGet("Advertiser{id}")]
    public IActionResult GetAdvertiser(int id)
    {
        var deal = DealService.FindByAdvertiserId(id);
        if (deal is null)
            return NotFound();
        return Ok(deal);
    }

    [HttpGet("Platform{id}")]
    public IActionResult GetPlatform(int id)
    {
        var deal = DealService.FindByPlatformId(id);
        if (deal is null)
            return NotFound();
        return Ok(deal);
    }
    [HttpGet]
    public IActionResult Get(int id)
    {
        return Ok(DealService.GetAll());
    }

    // [HttpPost("AddDeal")]
    // public IActionResult AddDeal(Deal deal)
    // {
    //     if (!ModelState.IsValid)
    //         return BadRequest();
        
    // }
    

}


