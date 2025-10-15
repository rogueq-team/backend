
using System.IdentityModel.Tokens.Jwt;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public static class JWTService
{
    
    public string GenerateToken(User user)
    {
        var token = JwtSecurityToken(
            new SigningCredentials
        )
    }
}
