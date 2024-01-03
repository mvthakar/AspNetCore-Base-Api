using BaseAPI.Database.Models;
using BaseAPI.Database.Models.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaseAPI.Database;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        var configuration = app.Configuration;
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        await context.Database.EnsureCreatedAsync();

        await CreateDefaultAuthProviders(configuration, context);
        await CreateDefaultTokenTypes(configuration, context);
        await CreateDefaultRoles(configuration, context, roleManager);
        await CreateDefaultUsers(configuration, context, userManager);
    }

    private static async Task CreateDefaultAuthProviders(IConfiguration configuration, DatabaseContext context)
    {
        if (context.AuthProviders.Any())
            return;

        string[] authProviders = configuration.GetSection("Defaults:AuthProviders").Get<string[]>()!;
        foreach (var authProvider in authProviders)
        {
            var authProviderExists = await context.AuthProviders.FirstOrDefaultAsync(item => item.Name == authProvider);
            if (authProviderExists == null)
                await context.AuthProviders.AddAsync(new AuthProvider() { Name = authProvider });
        }
    }

    private static async Task CreateDefaultTokenTypes(IConfiguration configuration, DatabaseContext context)
    {
        if (context.TokenTypes.Any())
            return;

        string[] tokenTypes = configuration.GetSection("Defaults:TokenTypes").Get<string[]>()!;
        foreach (var tokenType in tokenTypes)
        {
            var tokenTypeExists = await context.TokenTypes.FirstOrDefaultAsync(item => item.Name == tokenType);
            if (tokenTypeExists == null)
                await context.TokenTypes.AddAsync(new TokenType() { Name = tokenType });
        }
    }

    private static async Task CreateDefaultRoles(IConfiguration configuration, DatabaseContext context, RoleManager<Role> roleManager)
    {
        if (context.Roles.Any())
            return;

        string[] roleNames = configuration.GetSection("Defaults:Roles").Get<string[]>()!;
        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                await roleManager.CreateAsync(new Role(roleName));
        }
    }

    private static async Task CreateDefaultUsers(IConfiguration configuration, DatabaseContext context, UserManager<User> userManager)
    {
        if (context.Users.Any())
            return;

        var emailAuthProvider = await context.AuthProviders.FirstAsync(provider => provider.Name == "Email");
        foreach (var defaultUser in configuration.GetSection("Defaults:Users").GetChildren())
        {
            string email = defaultUser["Email"]!;
            string password = defaultUser["Password"]!;
            string role = defaultUser["Role"]!;
            bool emailConfirmed = defaultUser.GetValue<bool>("EmailConfirmed");

            var user = new User() { UserName = email, Email = email, EmailConfirmed = emailConfirmed, AuthProvider = emailAuthProvider };
            if (await userManager.FindByEmailAsync(email) == null)
            {
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
