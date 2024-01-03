namespace BaseAPI.Common.Settings;

public class JwtSettings
{
    public string Issuer { get; init; } = null!;
    public string SecurityKey { get; init; } = null!;
    public int AccessTokenExpiryInMinutes { get; init; }
    public int RefreshTokenExpiryInDays { get; init; }
}
