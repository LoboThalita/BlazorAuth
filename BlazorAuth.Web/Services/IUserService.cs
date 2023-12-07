namespace BlazorAuth.Web.Services;

public interface IUserService
{
    Task<string> LoginAsync(string login, string password);
}
