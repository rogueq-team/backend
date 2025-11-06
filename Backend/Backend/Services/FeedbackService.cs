using System.Text;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Enums;

namespace Backend.Services;

public class FeedbackService
{
    private readonly AppDbContext _db;

    public FeedbackService(AppDbContext context)
    {
        _db = context;
    }

    public async Task<FeedbackEntity?> FindbyFeedbackIdAsync(Guid id)
    {
        return await _db.Feedbacks.FirstOrDefaultAsync<FeedbackEntity>(feedback => feedback.Id == id);
    }

    public async Task<List<FeedbackEntity>> FindBySenderIdAsync(Guid senderId)
    {
        return await _db.Feedbacks.Where(feedback => feedback.SenderId == senderId).ToListAsync();
    }

    public async Task<List<FeedbackEntity>> FindByRecipientIdAsync(Guid recipientId)
    {
        return await _db.Feedbacks.Where(feedback => feedback.RecipientId == recipientId).ToListAsync();
    }
    public async Task<bool> CreateFeedbackAsync(FeedbackEntity feedback)
    {   
     try
        {
            feedback.Id = Guid.NewGuid();
            feedback.CreatedAt = DateTime.UtcNow;
            await _db.Feedbacks.AddAsync(feedback);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($" Ошибка добавления отзыва:");
            System.Console.WriteLine($"   Сообщение: {ex.Message}");

            if (ex.InnerException != null)
            {
                System.Console.WriteLine($"   Внутренняя ошибка: {ex.InnerException.Message}");

            }

            return false;
        }   
    }
    

    
}
