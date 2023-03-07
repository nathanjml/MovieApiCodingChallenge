using System.Reflection;
using AutoMapper;
using SimpleInjector;

namespace DestifyMovies.Core.Configuration
{
    public class AutoMapperProvider
    {
        private readonly Container _container;

        public AutoMapperProvider(Container container)
        {
            _container = container;
        }

        public IMapper GetMapper(params Assembly[] additionalAssemblies)
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.ConstructServicesUsing(_container.GetInstance);

            var assemblies = additionalAssemblies
                .Append(typeof(AutoMapperProvider).Assembly)
                .Distinct()
                .ToArray();

            configExpression.AddMaps(assemblies);

            var config = new MapperConfiguration(configExpression);
            return new Mapper(config, _container.GetInstance);
        }
    }
}
