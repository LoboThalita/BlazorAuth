using BlazorAuth.Api.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorAuth.Api.Service.JWTService;


public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly Jwt _jwt;

    public JwtTokenGenerator(Jwt jwt)
    {
        _jwt = jwt;
    }

    public async Task<string> GenerateTokenAsync(Usuario user)
    {
        var key = Encoding.ASCII.GetBytes(Key.Secret);


        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.email)
            };

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            _jwt.Issuer,
            null,
            claims,
            null,
            DateTime.UtcNow.AddHours(3),
            signingCredentials
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}

public sealed class Key
{
    public static string Secret = "6af0e4454e0a4cf08b38cd02c1dfe9c9";
}