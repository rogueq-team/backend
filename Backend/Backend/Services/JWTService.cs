
using System.IdentityModel.Tokens.Jwt;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class JWTService
{
    private readonly IConfiguration _configuration;
    public JWTService(IConfiguration conf) => _configuration = conf;
    public string GenerateToken(User user)
    {
        string? key = _configuration["JWTOptions:NoSecretKey"];
        var signingCredentials= new SigningCredentials(new SymmetricSecurityKey())
        var token = JwtSecurityToken(
            new SigningCredentials
        )
    }
}
