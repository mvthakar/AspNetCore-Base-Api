namespace BaseAPI.Common.Settings;

public static class Config
{
    public static IConfiguration Instance { get; private set; } = null!;

    public static void Persist(this IConfiguration configuration)
    {
        Instance = configuration;
    }
}
