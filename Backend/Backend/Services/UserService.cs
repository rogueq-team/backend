using System.Text;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Services;

public static class UserService
{
    static List<User> Users = new List<User>();
    static int id = 1;

    public static List<User> GetAll() => Users;

    public static User? FindByLogin(string? login)
    {
        if (string.IsNullOrEmpty(login)) return null;
        return Users.Find(User => User.Login == login);
    }

    public static User? FindByEmail(string? email)
    {
        if (string.IsNullOrEmpty(email)) return null;
        return Users.Find(User => User.Email == email);
    }
    public static User? FindById(int id)
    {
        
        return Users.Find(User => User.Id == id);
    }

    public static void Add(User user)
    {
        user.Id = id++;
        user.Password = PasswordService.HashPassword(user.Password);
        Users.Add(user);
    }

    public static void Delete(int id)
    {
        User? user = FindById(id);
        if (!(user is null))
            Users.Remove(user);

    }
    public static void Update(User user)
    {
        
        User? replaceUser = FindById(user.Id);
        if (!(replaceUser is null))
        {
            user.Password = PasswordService.HashPassword(user.Password);
            int index = Users.IndexOf(replaceUser);
            Users[index] = user;
        }
        

    }
}