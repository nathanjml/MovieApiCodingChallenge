using DesitfyMovies.Controllers;
using DestifyMovies.Core.Configuration;
using DestifyMovies.Core.Identity;
using DestifyMovies.Core.Services.Authorization;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.AspNetCore.Mvc.Controllers;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace DesitfyMovies.Configuration
{
    public static class SimpleInjectorConfig
    {
        public static Container _container;

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            _container.ConfigureCore(configuration);

            _container.RegisterInitializer<BaseApiController>(controller =>
            {
                controller.Mediator = _container.GetInstance<IMediator>();
            });

            _container.Register<IIdentityContext, HttpIdentityContext>(Lifestyle.Scoped);
            _container.Register<IIdentityContextFactory, HttpIdentityContextFactory>(Lifestyle.Scoped);

            services.AddSingleton(_container);
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.UseSimpleInjectorAspNetRequestScoping(_container);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient();

            services.AddSingleton(_ => _container.GetInstance<IMediator>());
            services.AddScoped(_ => _container.GetInstance<IApiKeyService>());


            services.AddSimpleInjector(_container, config =>
            {
                config.AddAspNetCore().AddControllerActivation();
                config.CrossWire<IHttpClientFactory>();
                config.CrossWire<IHttpContextAccessor>();

                config.AddLogging();
            });

            services.AddDistributedMemoryCache();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseSimpleInjector(_container);

            _container.Verify();
        }
    }
}
