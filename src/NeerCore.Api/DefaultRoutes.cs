namespace NeerCore.Api;

public static class DefaultRoutes
{
    public const string VersionedRoute = "/v{version:apiVersion}/[controller]";
    public const string LocalizedRoute = "/v{version:apiVersion}/{language:alpha:length(2)=en}/[controller]";
}