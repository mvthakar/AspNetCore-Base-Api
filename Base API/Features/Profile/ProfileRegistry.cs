using BaseAPI.Common;
using BaseAPI.Common.Utilities;
using BaseAPI.Features.Profile.Get;
using BaseAPI.Features.Profile.Update;
using BaseAPI.Features.Profile.UploadAvatar;

using FluentValidation;

namespace BaseAPI.Features.Profile;

public class ProfileRegistry : IRegistry
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapGet(Api.Url("profile"), GetProfileHandler.Handle).RequireAuthorization();
        app.MapPost(Api.Url("profile"), AddOrUpdateProfileHandler.Handle).RequireAuthorization();
        app.MapPost(Api.Url("profile/avatar"), UploadAvatarHandler.Handle)
            .Accepts<IFormFile>("multipart/form-data")
            .DisableAntiforgery()
            .RequireAuthorization();
    }

    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<IGetProfileService, GetProfileService>();
        
        services.AddScoped<IValidator<AddOrUpdateProfileRequest>, AddOrUpdateProfileRequestValidator>();
        services.AddScoped<IAddOrUpdateProfileService, AddOrUpdateProfileService>();

        services.AddScoped<IValidator<UploadAvatarRequest>, UploadAvatarRequestValidator>();
        services.AddScoped<IUploadAvatarService, UploadAvatarService>();
    }
}
