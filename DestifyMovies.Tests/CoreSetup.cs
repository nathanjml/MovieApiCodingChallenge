using System.IO;
using System.Reflection;
using DestifyMovies.Core.Configuration;
using DestifyMovies.Core.Identity;
using DestifyMovies.Tests.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DestifyMovies.Tests;

[SetUpFixture]
public class CoreSetup
{
    public static Container Container { get; private set; } = null!;

    [OneTimeSetUp]
    public static void CoreOneTimeSetup()
    {
        Container = new Container();
        Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        //inject appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json")
            .Build();

        //config assemblies for DI container registration
        var assemblyList = new[]
        {
            typeof(SimpleInjectorCoreConfig).Assembly,
            typeof(CoreSetup).Assembly
        };

        Container.RegisterSingleton(() => Substitute.For<ILogger>());

        Container.ConfigureCore(config, assemblyList);
        Container.Register<IIdentityContextFactory, MockIdentityContextFactory>(Lifestyle.Scoped);
    }
}