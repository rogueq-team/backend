using System.Text;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Enums;

namespace Backend.Services;

public class MessageService
{
    private readonly AppDbContext _db;

    public MessageService(AppDbContext context)
    {
        _db = context;
    }

    public async Task<bool> AddAsync(MessageEntity message)
    {
        try
        {
            message.Id = Guid.NewGuid();
            message.CreatedAt = DateTime.UtcNow;
            await _db.AddAsync(message);
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
  public async Task<List<MessageEntity>> GetMessagesByDealIdAsync(Guid dealId, int page = 1, int pageSize = 50)
{
    try
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 50;
        
        var skip = (page - 1) * pageSize;
        
        return await _db.Messages
            .Include(m => m.User) 
            .Include(m => m.Deal) 
            .Where(m => m.DealId == dealId ) 
            .OrderByDescending(m => m.CreatedAt) 
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking() 
            .ToListAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error getting messages for deal {dealId}: {ex}");
        return new List<MessageEntity>();
    }
}

public async Task<int> GetMessagesCountAsync(Guid dealId)
{
    try
    {
        return await _db.Messages
            .Where(m => m.DealId == dealId )
            .CountAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error counting messages for deal {dealId}: {ex}");
        return 0;
    }
}
    public async Task<MessageEntity?> FindByIdAsync(Guid Id)
    {
        return await _db.Messages.Include(m=>m.Deal).Include(m=>m.User).FirstOrDefaultAsync(m=>m.Id==Id);
    }
    public async Task<MessageEntity?> FindByUserId(Guid UserId)
    {
        return await _db.Messages.Include(m=>m.Deal).Include(m=>m.User).FirstOrDefaultAsync(m=>m.UserId==UserId);
    }
    public async Task<MessageEntity?> FindByDealId(Guid DealId)
    {
        return await _db.Messages.Include(m=>m.Deal).Include(m=>m.User).FirstOrDefaultAsync(m=>m.DealId==DealId);
    }
    
}
