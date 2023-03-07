using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DestifyMovies.Core.Domain;
public class MovieRating : Entity
{
    public int Rating { get; set; }
    public long MovieId { get; set; }
    public Movie? Movie { get; set; }
    public string? Comments { get; set; }
    public string RatingSubmittedBy { get; set; } = "";

    public class MovieRatingEntityConfiguration : IEntityTypeConfiguration<MovieRating>
    {
        public void Configure(EntityTypeBuilder<MovieRating> builder)
        {
            builder.HasOne(x => x.Movie)
                .WithMany(x => x.Ratings);
        }
    }
}
