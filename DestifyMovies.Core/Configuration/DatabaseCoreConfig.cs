using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Configuration;

public class DatabaseCoreConfig
{
    public static void Configure(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("DestifyMoviesDb");
    }
}