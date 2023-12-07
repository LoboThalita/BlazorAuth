using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using System.Net;
using BlazorAuth.Web.Services.Interfaces;

namespace BlazorAuth.Web.Services.Auth;


public class BlazorAppLoginService
{

    private readonly string TokenKey = nameof(TokenKey);

    private readonly ProtectedLocalStorage localStorage;
    private readonly NavigationManager navigation;
    private readonly IConfiguration configuration;
    private readonly IUserService _users;
    private readonly HttpClient _httpClient;


    public BlazorAppLoginService(ProtectedLocalStorage localStorage, NavigationManager navigation, IConfiguration configuration, HttpClient httpClient, IUserService users)
    {
        this.localStorage = localStorage;
        this.navigation = navigation;
        this.configuration = configuration;
        _httpClient = httpClient;
        _users = users;
    }

    public async Task<bool> LoginAsync(string email, string senha)
    {
        var isSuccess = false;

        var token = await _users.LoginAsync(email, senha);
        if (!string.IsNullOrEmpty(token))
        {
            isSuccess = true;
            await localStorage.SetAsync(TokenKey, token);
        }

        return isSuccess;
    }


    public async Task<List<Claim>> GetLoginInfoAsync()
    {
        var emptyResut = new List<Claim>();
        ProtectedBrowserStorageResult<string> token = default;
        try
        {
            token = await localStorage.GetAsync<string>(TokenKey);
        }
        catch (CryptographicException)
        {
            await LogoutAsync();
            return emptyResut;
        }

        if (token.Success && token.Value != default)
        {
            var claims = JwtTokenHelper.ValidateDecodeToken(token.Value, configuration);
            if (!claims.Any())
            {
                await LogoutAsync();
            }
            return claims;
        }
        return emptyResut;
    }

    public async Task LogoutAsync()
    {
        await RemoveAuthDataFromStorageAsync();
        navigation.NavigateTo("/", true);
    }


    private async Task RemoveAuthDataFromStorageAsync()
    {
        await localStorage.DeleteAsync(TokenKey);
    }
}