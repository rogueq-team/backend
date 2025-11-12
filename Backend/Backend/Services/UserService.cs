using System.Text;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Backend.Services;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext context)
    {
        _db = context;
    }

    public async Task<List<UserEntity>> GetAll() => await _db.Users.ToListAsync<UserEntity>();
    // public static List<RegUser> GetAllReg()
    // {
    //     List<RegUser> RegUsers = new();
    //     List<User> Users = GetAll();
    //     foreach (var User in Users)
    //     {
    //         RegUser regUser = new(User);
    //         RegUsers.Add(regUser);
    //     }
    //     return RegUsers;

    // }

    public async Task<UserEntity?> FindByLoginAsync(string? login)
    {
        if (string.IsNullOrEmpty(login)) return null;
        return await _db.Users.FirstOrDefaultAsync<UserEntity>(User => User.Login == login);
    }

    public async Task<UserEntity?> FindByEmailAsync(string? email)
    {
        if (string.IsNullOrEmpty(email)) return null;
        return await _db.Users.FirstOrDefaultAsync<UserEntity>(User => User.Email == email);
    }
    public async Task<UserEntity?> FindByIdAsync(Guid id)
    {

        return await _db.Users.FirstOrDefaultAsync<UserEntity>(User => User.UserId == id);
    }

    public async Task<bool> AddAsync(UserEntity user)
    {
        try
        {
            user.UserId = Guid.NewGuid();
            user.Password = PasswordService.HashPassword(user.Password);
            user.UpdatedAt=user.CreatedAt = DateTime.UtcNow;
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($" Ошибка добавления пользователя:");
            System.Console.WriteLine($"   Сообщение: {ex.Message}");

            if (ex.InnerException != null)
            {
                System.Console.WriteLine($"   Внутренняя ошибка: {ex.InnerException.Message}");


                if (ex.InnerException.Message.Contains("23505"))
                {
                    System.Console.WriteLine("  Нарушение уникальности: такой логин или email уже существует");
                }
            }

            return false;
        }
    }

    public void Delete(Guid id)
    {
        UserEntity? user = _db.Users.FirstOrDefault(u => u.UserId == id);
        if (user != null)
        {
            user.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();
        }

    }

    public async Task<bool> UpdateUserAsync(UserEntity updatedUser)
    {
        try
        {
            // Находим существующего пользователя
            UserEntity? existingUser = await _db.Users
                .FirstOrDefaultAsync(u => u.UserId == updatedUser.UserId);

            if (existingUser == null)
                return false;

            existingUser.Name = updatedUser.Name;
            existingUser.Login = updatedUser.Login;
            existingUser.Email = updatedUser.Email;
            if (!string.IsNullOrEmpty(updatedUser.Password) &&
                existingUser.Password != updatedUser.Password)
            {
                existingUser.Password = PasswordService.HashPassword(updatedUser.Password);
            }
            existingUser.Role = updatedUser.Role;
            existingUser.Type = updatedUser.Type;
            existingUser.Balance = updatedUser.Balance;
            existingUser.AvatarPath = updatedUser.AvatarPath;
            existingUser.Bio = updatedUser.Bio;
            existingUser.SocialLinks = updatedUser.SocialLinks;
            existingUser.IsVerified = updatedUser.IsVerified;

            existingUser.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении пользователя: {ex.Message}");
            return false;
        }
    }

}
