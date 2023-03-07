using DestifyMovies.Core.Features.Movies.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DestifyMovies.Core.Domain;
public class Movie : Entity
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Director { get; set; } = "";

    public DateOnly ReleasedOn { get; set; }
    public List<Actor> Actors { get; set; } = new();
    public List<MovieRating> Ratings { get; set; } = new();

    public class MovieEntityConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder
                .HasMany(x => x.Actors)
                .WithMany(x => x.Movies);
            
            //builder.HasData(MovieSeedData.GetSeedMovies());
        }
    }
}
