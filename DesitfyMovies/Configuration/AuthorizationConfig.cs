using DesitfyMovies.Authorization;

namespace DesitfyMovies.Configuration;

public static class AuthorizationConfig
{

    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        //serviceCollection.AddTransient<IAuthorizationHandler, ApiKeyRequirementHandler>();
        //serviceCollection.AddAuthorization(options =>
        //{
        //    options.AddPolicy("ApiKeyPolicy", policyBuilder =>
        //    {
        //        policyBuilder.AddRequirements(new ApiKeyRequirement());
        //    });
        //});

        serviceCollection.AddAuthentication("ApiKey")
            .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>("ApiKey", _ =>
            {
            });
    }
}