namespace DesitfyMovies.Configuration
{
    public static class LoggingConfig
    {
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(builder =>
            {
                builder.AddDebug()
                    .AddConsole();
            });
        }
    }
}
