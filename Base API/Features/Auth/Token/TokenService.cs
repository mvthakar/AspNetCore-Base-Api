using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BaseAPI.Common.Settings;
using BaseAPI.Database.Models.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using static BaseAPI.Common.Constants.Auth;

namespace BaseAPI.Features.Auth.Token;

public interface ITokenService
{
    Task<string> GenerateAccessToken(User user);
    string GenerateRefreshToken(out DateTime issuedOn, out DateTime expiresOn);
}

public class TokenService(UserManager<User> userManager, IOptions<JwtSettings> jwtSettings) : ITokenService
{
    public async Task<string> GenerateAccessToken(User user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var issedOn = DateTime.UtcNow;
        var expiresOn = DateTime.UtcNow.AddMinutes(jwtSettings.Value.AccessTokenExpiryInMinutes);

        var token = GenerateToken(issedOn, expiresOn,
        [
            new(Claims.UserId, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!.ToString()),
            new(ClaimTypes.Role, roles[0]),
            new(Claims.TokenType, TokenTypes.AccessToken)
        ]);

        return token;
    }

    public string GenerateRefreshToken(out DateTime issuedOn, out DateTime expiresOn)
    {
        issuedOn = DateTime.UtcNow;
        expiresOn = DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpiryInDays);

        return Guid.NewGuid().ToString();
    }

    private string GenerateToken(DateTime issuedOn, DateTime expiresOn, Claim[]? claims = null)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.SecurityKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = credentials,
            Expires = expiresOn,
            Issuer = jwtSettings.Value.Issuer,
            IssuedAt = issuedOn
        };

        if (claims is not null)
            tokenDescriptor.Subject = new ClaimsIdentity(claims);

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return token;
    }
}
