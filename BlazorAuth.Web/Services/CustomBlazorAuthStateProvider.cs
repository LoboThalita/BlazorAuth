using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorAuth.Web.Services;
public class CustomBlazorAuthStateProvider : AuthenticationStateProvider
{
    private bool authenticationChecked = true;
    private AuthenticationState cachedAuthenticationState;

    private readonly BlazorAppLoginService blazorAppLoginService;

    public CustomBlazorAuthStateProvider(BlazorAppLoginService blazorAppLoginService)
    {
        this.blazorAppLoginService = blazorAppLoginService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!authenticationChecked)
        {
            var claims = await blazorAppLoginService.GetLoginInfoAsync();
            ClaimsIdentity claimsIdentity;
            if (claims.Any())
            {
                claimsIdentity = new ClaimsIdentity(claims, "Bearer");
            }
            else
            {
                claimsIdentity = new ClaimsIdentity();
            }

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            cachedAuthenticationState = new AuthenticationState(claimsPrincipal);
        }
        else
        {
            cachedAuthenticationState = new AuthenticationState(new ClaimsPrincipal());
        }
        authenticationChecked = false;
        return cachedAuthenticationState;
    }

    public void MarkUserAsAuthenticated()
    {
        // Este método pode ser chamado para marcar manualmente o usuário como autenticado
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "usuario@exemplo.com"),
            new Claim(ClaimTypes.Role, "Admin"),
            // Adicione outras reivindicações conforme necessário
        }, "custom"));

        cachedAuthenticationState = new AuthenticationState(claimsPrincipal);
        NotifyAuthenticationStateChanged(Task.FromResult(cachedAuthenticationState));
    }

    public void MarkUserAsLoggedOut()
    {
        // Este método pode ser chamado para marcar manualmente o usuário como desconectado
        cachedAuthenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(cachedAuthenticationState));
    }
}
