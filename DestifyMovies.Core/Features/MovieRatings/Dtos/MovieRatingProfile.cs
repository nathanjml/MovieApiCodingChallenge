using AutoMapper;
using DestifyMovies.Core.Domain;

namespace DestifyMovies.Core.Features.MovieRatings.Dtos;

public class MovieRatingProfile : Profile
{
    public MovieRatingProfile()
    {
        CreateMap<MovieRating, ListMovieRatingDto>();
        CreateMap<MovieRating, MovieRatingGetDto>();
        CreateMap<MovieRatingDto, MovieRating>();
    }
}