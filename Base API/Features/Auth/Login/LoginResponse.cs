namespace BaseAPI.Features.Auth.Login;

public record LoginResponse(string AccessToken, string RefreshToken);
public record LoginWebResponse(string AccessToken);
