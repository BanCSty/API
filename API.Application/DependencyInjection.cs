using Microsoft.Extensions.DependencyInjection;

namespace API.Application
{
    //Для внедрение в ConfigureService нашего WebAPI
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
