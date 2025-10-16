
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class RefreshTokenService
{
    int id = 0;
    List<RefreshToken> Tokens = new List<RefreshToken>();
    IConfiguration _configuration;
    public RefreshTokenService(IConfiguration conf) => _configuration = conf;
    public string  CreateRefreshToken(int userId)
    {
        DeleteRefreshTokenById(userId);
        if (UserService.FindById(userId) is not null)
        {
            double time = 1;
            double.TryParse(_configuration["JWTOptions:Accestimeout"], out time);
            RefreshToken NewRefreshToken = new RefreshToken()
            {
                Id = id++,
                UserId = userId,
                Token = GenerateToken(),
                Expires = DateTime.UtcNow.AddDays(time),
                CreatedAt = DateTime.UtcNow
            };
            DeleteRefreshTokenById(userId);
            Tokens.Add(NewRefreshToken);
            return NewRefreshToken.Token;

        }
        return "";

    }
    public RefreshToken? GetRefreshTokenByUserId(int userId)
    {
        return Tokens.Find(rf => rf.UserId == userId);
    }
      public RefreshToken? GetRefreshTokenByToken(string token)
    {
        return Tokens.Find(rf => rf.Token ==token);
    }
    public string GetToken(RefreshToken token)
    {
        return token.Token;
    }
    public void DeleteRefreshTokenById(int userId)
    {
        Tokens.RemoveAll(us => us.UserId == userId);
    }
    public void DeleteRefreshTokenByToken(string rfToken)
    {
        var token = Tokens.FirstOrDefault(rf => rf.Token == rfToken);
        if (token is not null)
            Tokens.Remove(token);
    }
    public string GenerateToken()
    {

        var random = System.Security.Cryptography.RandomNumberGenerator.Create();
        var RandomNuber = new byte[32];
        random.GetBytes(RandomNuber);
        return Convert.ToBase64String(RandomNuber);
    }
}