using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Services.Mediator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Actors;

public class DeleteActorRequest : IRequest
{
    public long Id { get; set; }
}

public class DeleteActorRequestValidator : AbstractValidator<DeleteActorRequest>
{
    public DeleteActorRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteActorRequestHandler : IRequestHandler<DeleteActorRequest, NoResult>
{
    private readonly DbContext _dbContext;

    public DeleteActorRequestHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Response<NoResult>> HandleAsync(DeleteActorRequest request, CancellationToken token = default)
    {
        var actor = await _dbContext.Set<Actor>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, token);

        if (actor == null)
            return Response<NoResult>.NotFound();

        _dbContext.Set<Actor>()
            .Remove(actor);

        await _dbContext.SaveChangesAsync(token);

        return Response.Success();
    }
}