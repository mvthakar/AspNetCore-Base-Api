namespace BaseAPI.Common.Utilities;

public static class Api
{
    public static readonly string BasePath = "api";
    public static readonly string CurrentVersion = "v1";

    public static string Url(string endpoint) => $"/{BasePath}/{CurrentVersion}/{endpoint}";
}
