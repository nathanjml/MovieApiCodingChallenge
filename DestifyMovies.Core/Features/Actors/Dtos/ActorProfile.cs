using AutoMapper;
using DestifyMovies.Core.Domain;

namespace DestifyMovies.Core.Features.Actors.Dtos;

public class ActorProfile : Profile
{
    public ActorProfile()
    {
        CreateMap<Actor, ActorGetDto>();
        CreateMap<ActorDto, Actor>();
        CreateMap<ActorEditDto, Actor>();
    }
}