using AutoMapper;
using DestifyMovies.Core.Domain;

namespace DestifyMovies.Core.Features.Movies.Dtos
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<Movie, ListMovieDto>();
            CreateMap<Movie, MovieGetDto>();
            CreateMap<MovieDto, Movie>()
                .ForMember(x => x.ReleasedOn, (y) => y.MapFrom(z =>  DateOnly.Parse(z.ReleasedOn)));
            CreateMap<MovieEditDto, Movie>()
                .ForMember(x => x.ReleasedOn, (y) => y.MapFrom(z => DateOnly.Parse(z.ReleasedOn)));
        }
    }
}
