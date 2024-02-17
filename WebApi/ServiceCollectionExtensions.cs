using Domain.Interfaces.Generics;
using Domain.Interfaces.InterfaceServicos;
using System.Reflection;

namespace WebApi
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSingletonsImplementingInterface(this IServiceCollection services)
        {
            // var types = typeof(ICategoriaServico).Assembly.GetTypes();
            //.SelectMany(s => s.GetTypes())
            //.Where(p => p.IsClass && !p.IsAbstract);

            //types.ToList().ForEach(type =>
            //{
            //    if (type.IsInterface) { 
            //       // var implementations = types.Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableFrom(t));
            //        var implementation = types.Where(t => !t.IsInterface && !t.IsAbstract && t.GetInterfaces().Contains(type)).ToList();
            //        if (implementation.Count != 0)
            //        {
            //            services.AddSingleton(type, implementation);
            //        }
            //    }
            //}
            //);

            Type ti = typeof(ICategoriaServico);
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (ti.IsAssignableFrom(t))
                    {
                        // here's your type in t
                    }
                }
            }
        }
    }
}
