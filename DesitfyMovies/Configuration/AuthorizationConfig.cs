using DesitfyMovies.Authorization;

namespace DesitfyMovies.Configuration;

public static class AuthorizationConfig
{

    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication("ApiKey")
            .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>("ApiKey", _ =>
            {
            });
    }
}