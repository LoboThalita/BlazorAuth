using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorAuth.Web.Services.Auth;
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
        return new AuthenticationState(claimsPrincipal);
    }
}
