namespace BaseAPI.Common.Settings;

public class EmailSettings
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string SmtpHost { get; init; } = null!;
    public int SmtpPort { get; init; }
}
