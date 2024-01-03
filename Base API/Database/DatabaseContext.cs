using BaseAPI.Common.Settings;
using BaseAPI.Database.Configurations;
using BaseAPI.Database.Configurations.Identity;
using BaseAPI.Database.Models;
using BaseAPI.Database.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaseAPI.Database;

public class DatabaseContext(DbContextOptions options) : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserTokenIgnored>(options)
{
    public DbSet<AuthProvider> AuthProviders { get; set; }
    public DbSet<TokenType> TokenTypes { get; set; }
    public new DbSet<UserToken> UserTokens {  get; set; }
    public DbSet<Profile> Profiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = Config.Instance.GetConnectionString("Default");
        options.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Roles");
        builder.Entity<RoleClaim>().ToTable("RoleClaims");
        builder.Entity<UserClaim>().ToTable("UserClaims");
        builder.Entity<UserLogin>().ToTable("UserLogin");
        builder.Entity<UserRole>().ToTable("UserRoles");
        builder.Entity<UserTokenIgnored>().ToTable("__UserTokensIgnored");

        new UserEntityConfiguration().Configure(builder.Entity<User>());
        new RoleEntityConfiguration().Configure(builder.Entity<Role>());
        new UserTokenEntityConfiguration().Configure(builder.Entity<UserToken>());

        new AuthProviderEntityConfiguration().Configure(builder.Entity<AuthProvider>());
        new ProfileEntityConfiguration().Configure(builder.Entity<Profile>());
    }
}
