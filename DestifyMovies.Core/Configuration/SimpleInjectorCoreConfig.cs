using System.Reflection;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Services.Authorization;
using DestifyMovies.Core.Services.Mediator;
using DestifyMovies.Core.Services.Mediator.Decorators.Timer;
using DestifyMovies.Core.Services.Mediator.Decorators.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace DestifyMovies.Core.Configuration
{
    public static class SimpleInjectorCoreConfig
    {
        public static void ConfigureCore(this Container container, IConfiguration config,
            params Assembly[] assemblies)
        {
            var configAssemblies = new List<Assembly>(1 + assemblies.Length)
            {
                typeof(SimpleInjectorCoreConfig).GetTypeInfo().Assembly
            };

            configAssemblies.AddRange(assemblies);
            
            container.Register(() => config);

            ConfigureAutoMapper(container, configAssemblies);
            ConfigureDatabase(container);
            ConfigureMediatorHandlers(container, configAssemblies);

            container.Register<IApiKeyService, ApplicationKeyService>(Lifestyle.Scoped);
        }

        private static void ConfigureMediatorHandlers(Container container, IList<Assembly> assemblies)
        {
            container.Register<IMediator>(() => new Mediator(container.GetInstance));

            var shouldValidate = ShouldValidate();

            container.RegisterInstance(new ValidatorFactory(container));

            container.Register(typeof(IRequestHandler<>), assemblies);
            container.Register(typeof(IRequestHandler<,>), assemblies);

            container.Register(typeof(IValidator<>), assemblies);
            container.RegisterConditional(typeof(IRequestValidator<>), typeof(FluentValidatorAdapter<>), c => !c.Handled);

            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ValidationHandler<,>), Lifestyle.Transient, shouldValidate);
            container.RegisterDecorator(typeof(IRequestHandler<>), typeof(ValidationHandler<>), Lifestyle.Transient, shouldValidate);
            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TimerHandler<,>));
            container.RegisterDecorator(typeof(IRequestHandler<>), typeof(TimerHandler<>));

        }

        private static void ConfigureDatabase(Container container)
        {
            container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<DataContext>();
                DatabaseCoreConfig.Configure(options);
                return options.Options;
            });

            container.Register(() => new DataContext(
                container.GetInstance<DbContextOptions<DataContext>>()
            ), Lifestyle.Scoped);

            container.Register<DbContext>(container.GetInstance<DataContext>, Lifestyle.Scoped);
        }

        private static void ConfigureAutoMapper(Container container, IEnumerable<Assembly> configAssemblies)
        {
            var mapper = new AutoMapperProvider(container).GetMapper(configAssemblies.ToArray());
            container.RegisterSingleton(() => mapper);
        }

        private static Predicate<DecoratorPredicateContext> ShouldValidate()
        {
            return c => !c.ImplementationType.RequestHasAttribute(typeof(DoNotValidateAttribute));
        }
    }
}
