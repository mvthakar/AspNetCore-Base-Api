using BaseAPI.Common;
using BaseAPI.Common.Settings;
using BaseAPI.Database;
using BaseAPI.Database.Models.Identity;
using BaseAPI.Features.Auth;
using BaseAPI.Features.Profile;
using BaseAPI.Middleware;

using Microsoft.AspNetCore.Identity;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.Persist();

builder.Host.UseSerilog();
builder.Services.AddProblemDetails();

// ----Add services here-----

builder.Services.AddConfigurations();

builder.Services.AddCommonServices();
builder.Services.AddAuthServices();
builder.Services.AddProfileServices();

// --------------------------

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJwtAuthentication();
builder.Services.AddJwtAuthorization();

var app = builder.Build();
await app.InitializeDatabaseAsync();

app.UseSerilogRequestLogging();

// ----Map endpoints here-----

app.MapAuthEndpoints();
app.MapProfileEndpoints();

// --------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseStaticFiles();

app.UseForwardedHeaders();
app.UseExceptionHandler();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
await Log.CloseAndFlushAsync();
