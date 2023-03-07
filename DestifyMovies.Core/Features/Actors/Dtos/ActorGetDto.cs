namespace DestifyMovies.Core.Features.Actors.Dtos
{
    public class ActorGetDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string CountryOfOrigin { get; set; } = "";
    }
}
