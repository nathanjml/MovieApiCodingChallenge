using DestifyMovies.Core.Services.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Identity;

public interface IIdentityContext
{
    User? User { get; }
}

public interface IIdentityContextFactory
{
    IIdentityContext Create(IApiKeyService apiKeyService);
}