using BlazorAuth.Api.Entities;

namespace BlazorAuth.Api.Service.JWTService;

public interface IJwtTokenGenerator
{
    Task<string> GenerateTokenAsync(Usuario usuario);
}
