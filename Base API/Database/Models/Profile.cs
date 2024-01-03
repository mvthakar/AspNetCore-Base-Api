using BaseAPI.Database.Models.Identity;

namespace BaseAPI.Database.Models;

public class Profile
{
    public long Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Pincode { get; set; } = null!;
    public string? AvatarFileName { get; set; } = null;

    public long UserId { get; set; }
    public virtual User User { get; set; } = null!;
}
