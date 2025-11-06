using System.Reflection.Metadata;
using Backend.Entities;
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
            return await _context.Applications
                .Where(a => a.Status.ToLower() == status.ToLower() && a.DeletedAt == null)
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
        public async Task<bool> UpdateAsync(ApplicationEntity updated)
        {
            var existing = await FindByIdAsync(updated.ApplicationId);
            if (existing == null) return false;

            existing.Description = updated.Description;
            existing.Cost = updated.Cost;
            existing.Status = updated.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // мягкое удаление
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await FindByIdAsync(id);
            if (existing == null) return false;

            existing.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
