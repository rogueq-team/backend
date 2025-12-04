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
            await _db.
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
    
}
