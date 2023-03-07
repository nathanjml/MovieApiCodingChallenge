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
        public static Container Container = new();

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            Container.ConfigureCore(configuration);

            Container.RegisterInitializer<BaseApiController>(controller =>
            {
                controller.Mediator = Container.GetInstance<IMediator>();
            });

            Container.Register<IIdentityContext, HttpIdentityContext>(Lifestyle.Scoped);
            Container.Register<IIdentityContextFactory, HttpIdentityContextFactory>(Lifestyle.Scoped);

            services.AddSingleton(Container);
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(Container));
            services.UseSimpleInjectorAspNetRequestScoping(Container);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient();

            services.AddSingleton(_ => Container.GetInstance<IMediator>());
            services.AddScoped(_ => Container.GetInstance<IApiKeyService>());


            services.AddSimpleInjector(Container, config =>
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
            app.UseSimpleInjector(Container);

            Container.Verify();
        }
    }
}
