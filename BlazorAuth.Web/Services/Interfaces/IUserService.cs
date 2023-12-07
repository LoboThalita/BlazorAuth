namespace BlazorAuth.Web.Services.Interfaces;

public interface IUserService
{
    Task<string> LoginAsync(string login, string password);
}
