using BlazorAuth.Api.Entities;

namespace BlazorAuth.Api.Service.Autenticacao;

public interface IAuthenticationService
{
    Task<string?> Autenticar(LoginRequest loginRequest);
    Task Registrar(Usuario usuario);
}
