
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class JWTService
{
    private readonly IConfiguration _configuration;
    public JWTService(IConfiguration conf) => _configuration = conf;

    public string GenerateAccesToken(UserEntity user)
    {
        Claim[] claims = [new(
            "UserId", user.UserId.ToString()),
            new ("Email", user.Email),
            new ("login", user.Login),
            new("Role", (int)user.Role==0?"Admin":"User"),
            new("UserType", (int)user.Type==0?"Platform":(int)user.Type==1?"Advertiser":"Both")];
        string key = _configuration["JWTOptions:NoSecretKey"];
        double time = 1;
        double.TryParse(_configuration["JWTOptions:Accestimeout"], out time);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(time));
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }

}
