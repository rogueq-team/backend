using System.Reflection.Metadata;
using Backend.Entities;
using Backend.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ApplicationService
    {
        private readonly AppDbContext _context;

        public ApplicationService(AppDbContext context)
        {
            _context = context;
        }

        // все заявки
        public async Task<List<ApplicationEntity>> GetAllAsync()
        {
            return await _context.Applications
                .Where(a => a.DeletedAt == null)
                .ToListAsync();
        }

        // поиск по ID
        public async Task<ApplicationEntity?> FindByIdAsync(Guid id)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.ApplicationId == id && a.DeletedAt == null);
        }

        // поиск по статусу
        public async Task<List<ApplicationEntity>> FindByStatusAsync(string status)
        {
            if (!Enum.TryParse<ApplicationStatus>(status, true, out var parsedStatus))
                throw new ArgumentException("Некорректный статус");

            return await _context.Applications
                .Where(a => a.Status == parsedStatus && a.DeletedAt == null)
                .ToListAsync();
        }

        // поиск по пользователю
        public async Task<List<ApplicationEntity>> FindByUserIdAsync(Guid userId)
        {
            return await _context.Applications
                .Where(a => a.UserId == userId && a.DeletedAt == null)
                .ToListAsync();
        }

        // добавить новую заявку
        public async Task<ApplicationEntity> AddAsync(ApplicationEntity application)
        {
            application.CreatedAt = DateTime.UtcNow;
            application.UpdatedAt = application.CreatedAt;
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        // обновление существующей заявки
        public async Task<bool> UpdateAsync(Guid id, ApplicationEntity updated, Guid userId, string userRole)
        {
            var existing = await FindByIdAsync(id);
            if (existing == null) return false;

            if (userRole != "Admin" && existing.UserId != userId)
                return false;

            existing.Description = updated.Description;
            existing.Cost = updated.Cost;
            existing.Status = updated.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // заявки по ид
        public async Task<List<ApplicationEntity>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Applications
                                 .Where(a => a.UserId == userId)
                                 .Where(a => a.DeletedAt == null)
                                 .ToListAsync();
        }


        // мягкое удаление
        public async Task<bool> DeleteAsync(Guid id, Guid userId, string userRole)
        {
            var existing = await FindByIdAsync(id);
            if (existing == null) return false;

            if (userRole != "Admin" && existing.UserId != userId)
                return false;

            existing.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
