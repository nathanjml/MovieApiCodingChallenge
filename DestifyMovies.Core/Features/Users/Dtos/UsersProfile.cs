using AutoMapper;
using DestifyMovies.Core.Identity;

namespace DestifyMovies.Core.Features.Users.Dtos;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}