using Microsoft.AspNetCore.Identity;

namespace BaseAPI.Database.Models.Identity;

public class User : IdentityUser<long>
{
    public DateTime CreatedOnDateTime { get; set; }

    public long AuthProviderId { get; set; }
    public virtual AuthProvider AuthProvider { get; set; } = null!;

    public virtual Profile Profile { get; set; } = null!;
}
