using Microsoft.AspNetCore.Identity;

namespace BaseAPI.Database.Models.Identity;

public class UserRole : IdentityUserRole<long>
{

}

public class RoleClaim : IdentityRoleClaim<long>
{

}

public class UserClaim : IdentityUserClaim<long>
{

}

public class UserLogin : IdentityUserLogin<long>
{

}

public class UserTokenIgnored : IdentityUserToken<long>
{

}
