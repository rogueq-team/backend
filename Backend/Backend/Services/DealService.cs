using System.Text;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Enums;

namespace Backend.Services;

public class DealService
{
    private readonly AppDbContext _db;

    public DealService(AppDbContext context)
    {
        _db = context;
    }

    public List<DealEntity> GetAll() => _db.Deals.ToList<DealEntity>();

    public async Task<DealEntity?> FindByDealIdAsync(Guid dealId)
    {
        return await _db.Deals.FirstOrDefaultAsync<DealEntity>(deal => deal.DealId == dealId);
    }
    public async Task<List<DealEntity>> FindByApplicationIdAsync(Guid applicationId)
    {
        return await _db.Deals.Where(deal => deal.ApplicationId == applicationId).ToListAsync();
    }

    public async Task<List<DealEntity>> FindByAdvertiserIdAsync(Guid advertiserId)
    {
        return await _db.Deals.Where(deal => deal.AdvertiserId == advertiserId).ToListAsync();
    }
    public async Task<List<DealEntity>> FindByPlatformIdAsync(Guid platformId)
    {
        return await _db.Deals.Where(deal => deal.PlatformId == platformId).ToListAsync();
    }

    public async Task<bool> AddAsync(DealEntity deal)
    {
        try
        {
            deal.DealId = Guid.NewGuid();
            deal.CreatedAt = DateTime.UtcNow;
            await _db.Deals.AddAsync(deal);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($" Ошибка добавления сделки:");
            System.Console.WriteLine($"   Сообщение: {ex.Message}");

            if (ex.InnerException != null)
            {
                System.Console.WriteLine($"   Внутренняя ошибка: {ex.InnerException.Message}");

            }

            return false;
        }
    }

    public async Task<bool> ChangeStatusById(Guid dealId, string status)
    {
        try
        {
            DealEntity? deal = await FindByDealIdAsync(dealId);
            if (deal == null)
                return false;
            if (Enum.TryParse<DealStatus>(status, out DealStatus dealStatus))
            {
                deal.Status = dealStatus;
            }
            else
            {
                return false;
            }
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении статуса сделки: {ex.Message}");
            return false;
        }
    }
    public async Task<bool> UpdateDealAsync(DealEntity updatedDeal)
    {
        try
        {

            var UpdatedDeal = await FindByDealIdAsync(updatedDeal.DealId);

            if (UpdatedDeal == null)
                return false;

            UpdatedDeal.Description = updatedDeal.Description;
            UpdatedDeal.Status = updatedDeal.Status;

            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка обновления сделки: {ex.Message}");
            return false;
        }
    }
}
