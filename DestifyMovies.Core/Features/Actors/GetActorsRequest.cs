using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Actors.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Actors;

public class GetActorsRequest : IRequest<List<ActorGetDto>>
{
    public string? Name { get; set; }
}

public class GetActorsRequestHandler : IRequestHandler<GetActorsRequest, List<ActorGetDto>>
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;

    public GetActorsRequestHandler(DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Response<List<ActorGetDto>>> HandleAsync(GetActorsRequest request, CancellationToken token = default)
    {
        var actors = await _dbContext.Set<Actor>()
            .AsNoTracking()
            .AsQueryable()
            .WhereIf(() => request.Name != null
                , (x) => $"{x.FirstName} {x.LastName}".ToLowerInvariant().Contains(request.Name!.ToLowerInvariant()))
            .ToListAsync(token);

        return actors
            .Select(_mapper.Map<ActorGetDto>)
            .ToList()
            .ToResponse();
    }
}