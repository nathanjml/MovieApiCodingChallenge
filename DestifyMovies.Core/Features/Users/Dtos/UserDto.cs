namespace DestifyMovies.Core.Features.Users.Dtos;

public class UserDto
{
    public string EmailAddress { get; set; } = "";
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}