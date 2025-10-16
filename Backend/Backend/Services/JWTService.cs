
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class JWTService
{
    private readonly IConfiguration _configuration;
    public JWTService(IConfiguration conf) => _configuration = conf;

    public string GenerateAccesToken(User user)
    {
        Claim[] claims = [new("UserId", user.Id.ToString()), new("Role", user.Role), new("UserType", user.UserType)];
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
