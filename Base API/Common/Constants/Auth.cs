namespace BaseAPI.Common.Constants;

public static class Auth
{
    public static class Providers
    {
        public static readonly string Email = "Email";
    }

    public static class Jwt
    {
        public static readonly string Default = "Default";
        public static readonly string AllowExpired = "AllowExpiredJwt";
        public static readonly string RequireUserId = "RequireUserId";
    }

    public static class Roles
    {
        public static readonly string Admin = "Admin";
        public static readonly string User = "User";
    }

    public static class Claims
    {
        public static readonly string UserId = "UserId";
        public static readonly string TokenType = "TokenType";
    }

    public class TokenTypes
    {
        public static readonly string AccessToken = "AccessToken";
        public static readonly string RefreshToken = "RefreshToken";
        public static readonly string AuthToken = "AuthToken";
    }
}
