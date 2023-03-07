namespace DestifyMovies.Core.Features.Movies.Dtos;

public class MovieDto
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";

    public string ReleasedOn { get; set; } = "";
    public string Director { get; set; } = "";
    public long[]? ActorIds { get; set; }
}