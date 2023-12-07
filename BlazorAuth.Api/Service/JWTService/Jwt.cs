namespace BlazorAuth.Api.Service.JWTService;

public class Jwt
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; }
    public string Aud { get; set; }
}
