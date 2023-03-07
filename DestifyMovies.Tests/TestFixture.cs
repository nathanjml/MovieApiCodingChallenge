using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Services.Authorization;
using DestifyMovies.Core.Services.Mediator;
using NUnit.Framework;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DestifyMovies.Tests;

public class TestFixture
{
    private Scope _scope = null!;
    protected Container Container => CoreSetup.Container;
    protected IMediator Mediator { get; private set; } = null!;
    protected DataContext DbContext { get; private set; } = null!;
    protected IApiKeyService ApiKeyService { get; private set; } = null!;

    protected IMapper Mapper { get; private set; } = null!;

    [SetUp]
    public void BaseSetUp()
    {
        _scope = AsyncScopedLifestyle.BeginScope(Container);

        Mediator = Container.GetInstance<IMediator>();
        DbContext = Container.GetInstance<DataContext>();
        Mapper = Container.GetInstance<IMapper>();
        ApiKeyService = Container.GetInstance<IApiKeyService>();

        DbContext.Database.EnsureDeleted();
    }

    [TearDown]
    public void BaseTearDown()
    {
        _scope.Dispose();
    }
}