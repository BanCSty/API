using API.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.DAL
{
    public static class DependencyInjection
    {
        //Добавление контекста БД и его регистрация 
        public static IServiceCollection AddPersistence(this IServiceCollection service,
            IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
            service.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
            service.AddScoped<IApiDbContext>(provider =>
                provider.GetService<ApiDbContext>());

            return service;
        }
    }
}
