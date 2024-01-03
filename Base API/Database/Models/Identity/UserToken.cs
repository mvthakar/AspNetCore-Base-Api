namespace BaseAPI.Database.Models.Identity;

public class UserToken
{
    public long Id { get; set; }

    public long UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public long AuthProviderId { get; set; }
    public virtual AuthProvider AuthProvider { get; set; } = null!;

    public long TokenTypeId { get; set; }
    public virtual TokenType TokenType { get; set; } = null!;

    public string Value { get; set; } = null!;

    public DateTime IssuedOnDateTime { get; set; }
    public DateTime? ExpiresOnDateTIme { get; set; }
}