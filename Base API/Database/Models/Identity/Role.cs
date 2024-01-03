using Microsoft.AspNetCore.Identity;

namespace BaseAPI.Database.Models.Identity;

public class Role : IdentityRole<long>
{
    public Role() : base()
    {

    }

    public Role(string name) : base(name)
    {

    }
}
