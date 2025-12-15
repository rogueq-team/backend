using System.Text;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Enums;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class FeedbackService
{
    private readonly AppDbContext _db;

    public FeedbackService(AppDbContext context)
    {
        _db = context;
    }

    public async Task<FeedbackEntity?> FindByIdAsync(Guid id)
    {
        return await _db.Feedbacks.Include(f => f.Sender).Include(f => f.Recipient).FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<FeedbackEntity>> FindBySenderIdAsync(Guid senderId)
    {
        return await _db.Feedbacks.Where(f => f.SenderId == senderId).OrderByDescending(f => f.CreatedAt).ToListAsync();
    }

    public async Task<List<FeedbackEntity>> FindByRecipientIdAsync(Guid recipientId)
    {
        return await _db.Feedbacks.Where(f => f.RecipientId == recipientId).OrderByDescending(f => f.CreatedAt).ToListAsync();
    }

    public async Task<(bool Success, string Message)> CreateFeedbackAsync(
        Guid dealId,
        Guid senderId,
        int stars,
        string text)
    {
        var deal = await _db.Deals.FirstOrDefaultAsync(d => d.DealId == dealId);
        if (deal == null)
            return (false, "Сделка не найдена");

        if (deal.AdvertiserId != senderId && deal.PlatformId != senderId)
            return (false, "Вы не участник этой сделки");

        if (deal.Status != DealStatus.Completed && deal.Status != DealStatus.Canceled)
            return (false, "Отзыв можно оставить только после завершения сделки");

        if (string.IsNullOrWhiteSpace(text))
            return (false, "Текст отзыва не может быть пустым");

        if (text.Length > 100)
            return (false, "Текст отзыва не должен превышать 100 символов");

        if (stars < 1 || stars > 5)
            return (false, "Оценка должна быть от 1 до 5");

        Guid recipientId = deal.AdvertiserId == senderId ? deal.PlatformId : deal.AdvertiserId;

        bool exists = await _db.Feedbacks.AnyAsync(f => f.DealId == dealId && f.SenderId == senderId);

        if (exists)
            return (false, "Отзыв по этой сделке уже оставлен");

        var feedback = new FeedbackEntity
        {
            Id = Guid.NewGuid(),
            DealId = dealId,
            SenderId = senderId,
            RecipientId = recipientId,
            Stars = stars,
            Text = text,
            CreatedAt = DateTime.UtcNow
        };

        _db.Feedbacks.Add(feedback);
        await _db.SaveChangesAsync();

        return (true, "Отзыв успешно создан");
    }

    public async Task<(bool Success, string Message)> DeleteFeedbackAsync(
        Guid feedbackId,
        UserRole role)
    {
        if (role != UserRole.Admin)
            return (false, "Только администратор может удалять отзывы");

        var feedback = await _db.Feedbacks.FindAsync(feedbackId);
        if (feedback == null)
            return (false, "Отзыв не найден");

        _db.Feedbacks.Remove(feedback);
        await _db.SaveChangesAsync();

        return (true, "Отзыв удалён");
    }

}
