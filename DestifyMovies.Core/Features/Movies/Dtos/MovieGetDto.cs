namespace DestifyMovies.Core.Features.Movies.Dtos;

public class MovieGetDto
{
    public long Id { get; set; }
    public string Description { get; set; } = "";
    public string Title { get; set; } = "";
    public string Director { get; set; } = "";
    public DateOnly ReleasedOn { get; set; }
}