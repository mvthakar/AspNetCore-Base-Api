using System.Text;

using BaseAPI.Common.Constants;
using BaseAPI.Common.Services;
using BaseAPI.Common.Settings;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

using Serilog;

namespace BaseAPI.Common;

public static class CommonRegistry
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        
        return services;
    }

    public static IServiceCollection AddConfigurations(this IServiceCollection services)
    {
        services
            .Configure<JwtSettings>(Config.Instance.GetSection("JwtSettings"))
            .Configure<EmailSettings>(Config.Instance.GetSection("EmailSettings"));

        return services;
    }

    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        static JwtBearerOptions Configure(JwtBearerOptions options, bool validateLifetime)
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = validateLifetime,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Instance["JwtSettings:SecurityKey"]!)),
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = Config.Instance["JwtSettings:Issuer"]!
            };

            return options;
        }

        var authBuilder = services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        authBuilder.AddJwtBearer(Auth.Jwt.Default, options => Configure(options, validateLifetime: true));
        authBuilder.AddJwtBearer(Auth.Jwt.AllowExpired, options => Configure(options, validateLifetime: false));
    }

    public static void AddJwtAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(Auth.Jwt.Default)
                .Build();

            var allowExpiredJwtPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(Auth.Jwt.AllowExpired)
                .Build();

            options.AddPolicy(Auth.Jwt.Default, defaultPolicy);
            options.AddPolicy(Auth.Jwt.AllowExpired, allowExpiredJwtPolicy);

            options.AddPolicy(Auth.Roles.Admin, options => options.RequireRole(Auth.Roles.Admin));
            options.AddPolicy(Auth.Roles.User, options => options.RequireRole(Auth.Roles.User));

            options.DefaultPolicy = options.GetPolicy(Auth.Jwt.Default)!;
        });
    }

    public static void UseSerilog(this ConfigureHostBuilder host)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Config.Instance)
            .CreateLogger();

        host.UseSerilog(logger);
    }
}
