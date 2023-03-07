using DestifyMovies.Core.Configuration;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Features.Movies.Seed;
using Microsoft.EntityFrameworkCore;

namespace DesitfyMovies.Configuration;

public static class DatabaseConfig
{
    public static void ConfigureServices(IServiceCollection servicesCollection, IConfiguration config)
    {
        servicesCollection.AddDbContext<DataContext>(DatabaseCoreConfig.Configure);
    }

    public static void Configure(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetService<DataContext>();

        //seeds
        MovieSeedData.AddSeedData(context!);
    }
}