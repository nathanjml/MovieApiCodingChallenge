using DestifyMovies.Core.Services.Mediator;

namespace DestifyMovies.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasAttribute(this Type type, Type attributeType, bool inherit = true)
        {
            if (type.CustomAttributes.Any(x => attributeType.IsAssignableFrom(x.AttributeType)))
            {
                return true;
            }

            if (inherit && type.BaseType != null && type.BaseType.HasAttribute(attributeType))
            {
                return true;
            }

            return inherit && type.GetInterfaces().Any(x => x.HasAttribute(attributeType));
        }

        public static bool RequestHasAttribute(this Type type, Type attributeType, bool inherit = true)
        { 
            return type
                .GetInterfaces()
                .Where(x => x.Name == typeof(IRequestHandler<,>).Name ||
                            x.Name == typeof(IRequestHandler<>).Name)
                .Any(type => type
                    .GetGenericArguments()[0]
                    .HasAttribute(attributeType, inherit));
        }
    }
}
