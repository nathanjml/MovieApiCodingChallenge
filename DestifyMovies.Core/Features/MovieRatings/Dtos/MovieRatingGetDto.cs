namespace DestifyMovies.Core.Features.MovieRatings.Dtos
{
    public class MovieRatingGetDto
    {
        public string? Comments { get; set; }
        public int Rating { get; set; }
        public long MovieId { get; set; }
        public string RatingSubmittedBy { get; set; } = "";
    }
}
