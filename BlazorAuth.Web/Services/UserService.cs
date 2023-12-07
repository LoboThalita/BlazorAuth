using BlazorAuth.Web.Data;
using BlazorAuth.Web.Services.Auth;
using BlazorAuth.Web.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorAuth.Web.Services;

public class UserService : IUserService
{
    private readonly IConfiguration configuration;

    public UserService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<string> LoginAsync(string login, string password)
    {
        await Task.CompletedTask;

        UserDto user = null;
        var adminUser = new UserDto()
        {
            Login = "admin",
            Roles = UserRoles.Admin,
            Password = "1234"
        };
        var regularUser = new UserDto()
        {
            Login = "user",
            Roles = UserRoles.RegularUser,
            Password = configuration.GetValue<string>("UserPwd")
        };
        if (login == adminUser.Login && password == adminUser.Password)
        {
            user = adminUser;
        }
        else if (login == regularUser.Login && password == regularUser.Password)
        {
            user = regularUser;
        }

        if (user != null)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in user.Roles.FromFlagsToList())
            {
                if (userRole == UserRoles.None)
                {
                    continue;
                }

                authClaims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRoles), userRole).ToLowerInvariant(),
                    ClaimValueTypes.String));
            }

            var token = JwtTokenHelper.MakeToken(authClaims, configuration);

            return token;
        }

        return string.Empty;
    }
}