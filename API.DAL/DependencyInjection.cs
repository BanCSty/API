using API.DAL.Interfaces;
using API.DAL.Repositories;
using API.Domain;
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

            service.AddScoped<IBaseRepository<Founder>, FounderRepository>();
            service.AddScoped<IBaseRepository<LegalEntity>, LegalEntityRepository>();
            service.AddScoped<IBaseRepository<IndividualEntrepreneur>, IndividualEntrepreneurRepository>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            return service;
        }
    }
}
