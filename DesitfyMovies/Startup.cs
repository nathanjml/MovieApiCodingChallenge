using DesitfyMovies.Configuration;

namespace DesitfyMovies;

public class Startup
{
    private readonly IConfiguration _configuration;

    private Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static Startup InitializeFromBuilder(WebApplicationBuilder builder) => new(builder.Configuration);

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        MvcConfig.ConfigureServices(serviceCollection);
        SimpleInjectorConfig.ConfigureServices(serviceCollection, _configuration);
        AuthorizationConfig.ConfigureServices(serviceCollection);
        DatabaseConfig.ConfigureServices(serviceCollection, _configuration);
        SwaggerConfig.ConfigureServices(serviceCollection);
        //LoggingConfig.ConfigureServices(serviceCollection);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        SimpleInjectorConfig.Configure(app);
        MvcConfig.Configure(app, env);
        DatabaseConfig.Configure(app);
        SwaggerConfig.Configure(app);
    }
}