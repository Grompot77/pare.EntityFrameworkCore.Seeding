using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace pare.EntityFrameworkCore.Seed
{
    public static class SeedingCollectionExtensions
    {
        public static IServiceCollection AddSeeding<TSeeding>(this IServiceCollection serviceCollection)
            where TSeeding : ISeedData
        {
            ServiceCollectionDescriptorExtensions.TryAdd(serviceCollection, new ServiceDescriptor(typeof(TSeeding), typeof(TSeeding), ServiceLifetime.Singleton));
            return serviceCollection;
        }
    }
}
