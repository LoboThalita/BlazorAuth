using BlazorAuth.Api.Entities;
using BlazorAuth.Api.Repositories;
using BlazorAuth.Api.Service.JWTService;
using Microsoft.AspNetCore.Authentication;

namespace BlazorAuth.Api.Service.Autenticacao;
public class AuthenticationService : IAuthenticationService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUsuarioRepository usuarioRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<string?> Autenticar(LoginRequest loginRequest)
    {
        var usuario = await _usuarioRepository.GetByEmail(loginRequest.Email);
        if (usuario is null) return null;

        var token = await _jwtTokenGenerator.GenerateTokenAsync(usuario);
        return token;
    }

    public async Task Registrar(Usuario usuario)
    {
        await _usuarioRepository.Create(usuario);
    }
}
