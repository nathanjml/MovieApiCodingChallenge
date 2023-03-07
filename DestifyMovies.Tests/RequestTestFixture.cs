using System.Net;
using System.Threading.Tasks;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DestifyMovies.Tests;

public abstract class CreateRequestTestFixture<TRequest, TResult, TEntity> : TestFixture
    where TRequest : IRequest<TResult> 
    where TEntity : class
{
    protected IRequest<TResult> Request = null!;
    protected IRequestHandler<TRequest, TResult> Handler = null!;

    public abstract TRequest InitializeRequest();
    public abstract Task SetupAsync();

    [SetUp]
    public async Task TestSetUp()
    {
        await SetupAsync();
        Handler = Container.GetInstance<IRequestHandler<TRequest, TResult>>();
        Request = InitializeRequest();
    }

    [Test]
    public async Task ValidRequest_CreatesNewEntity()
    {
        var currentCount = await DbContext.Set<TEntity>().CountAsync();

        var response = await Handler.HandleAsync((TRequest)Request, default);

        Assert.IsFalse(response.HasErrors);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.AreEqual(currentCount + 1, await DbContext.Set<TEntity>().CountAsync());
    }
}