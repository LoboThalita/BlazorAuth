using BlazorAuth.Api.Entities;
using BlazorAuth.Api.Service.Autenticacao;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAuth.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var token = await _authenticationService.Autenticar(loginRequest);

        return token is null ? BadRequest("O e-mail ou senha fornecidos estão incorretos.") : Ok(token);
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(Usuario usuario)
    {
        await _authenticationService.Registrar(usuario);
        return Ok("Usuário cadastrado com sucesso");
    }
}