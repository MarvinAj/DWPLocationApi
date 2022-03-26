using com.dwp.user.location.Services;
using Microsoft.Extensions.DependencyInjection;

namespace com.dwp.user.location.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDWPLocationService(
            this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(p => p);
            serviceCollection.AddSingleton<ILocationStorage, LocationStorage>();
            serviceCollection.AddSingleton<ILocationService, LocationService>();
        }
    }
}
