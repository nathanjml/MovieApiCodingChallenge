using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DestifyMovies.Core.Domain;
public class Actor : Entity
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string CountryOfOrigin { get; set; } = "";
    public List<Movie> Movies { get; set; } = new List<Movie>();
}

public class ActorEntityConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
    }
}
