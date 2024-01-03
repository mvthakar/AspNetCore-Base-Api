namespace BaseAPI.Features.Auth.Token.Refresh;

public record RefreshTokenWebResponse(string AccessToken);
public record RefreshTokenResponse(string AccessToken, string RefreshToken);

