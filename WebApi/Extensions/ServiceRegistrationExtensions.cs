using Domain.Interfaces.Generics;
using FluentValidation;
using System.Reflection;

namespace WebApi.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static void InjecaoDependencia(this IServiceCollection services)
        {
            var assemblyNames = new[] { "Infra", "Domain" };
            RegisterInjectables(services, assemblyNames);
            
            RegisterValidators(services, Assembly.GetExecutingAssembly());            
        }

        private static void RegisterInjectables(IServiceCollection services, IEnumerable<string> assemblyNames)
        {
            foreach (var assemblyName in assemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                var injectableTypes = assembly.GetTypes()
                    .Where(type => typeof(IInjectable).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

                foreach (var type in injectableTypes)
                {
                    foreach (var iface in type.GetInterfaces())
                    {
                        services.AddScoped(iface, type);
                    }
                }
            }
        }

        private static void RegisterValidators(IServiceCollection services, Assembly assembly)
        {
            var validatorType = typeof(IValidator<>);
            var validatorTypes = assembly.GetTypes()
                .Where(type => type.GetInterfaces().Any(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == validatorType));

            foreach (var type in validatorTypes)
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsGenericType && iface.GetGenericTypeDefinition() == validatorType)
                    {
                        services.AddScoped(iface, type);
                    }
                }
            }
        }
    }
}
