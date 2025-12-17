
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class RefreshTokenService
{
    static int id = 0;
    static List<RefreshToken> Tokens = new List<RefreshToken>();
    public IConfiguration _configuration;
    public UserService _userService;
    

    public RefreshTokenService(IConfiguration cfg,UserService usrvs)
    {
        _configuration = cfg;
        _userService = usrvs;
    }
   
     public async Task<string> CreateRefreshToken(Guid  userId)
    {
        DeleteRefreshTokenByUserId(userId);
        if ( (await _userService.FindByIdAsync(userId)) is not null)
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
            DeleteRefreshTokenByUserId(userId);
            Tokens.Add(NewRefreshToken);
            return NewRefreshToken.Token;

        }
        return "";

    }

     public List<RefreshToken> GetAll() { return Tokens; }
    
     public RefreshToken? GetRefreshTokenByUserId(Guid userId)
    {
        return Tokens.Find(rf => rf.UserId == userId);
    }
      public RefreshToken? GetRefreshTokenByToken(string token)
    {
        System.Console.WriteLine(token);
        return Tokens.FirstOrDefault(rf => rf.Token == token);
        
    }
     public string GetToken(RefreshToken token)
    {
        return token.Token;
    }
     public void DeleteRefreshTokenByUserId(Guid userId)
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