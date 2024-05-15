using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace API.Application.Common.Mappings
{
    //Используется для автоматической загрузки и применения профилей маппинга из сборки
    public class AssemblyMappingProfile : Profile
    {
        //Представляет собой сборку, из которой нужно загрузить профили маппинга
        public AssemblyMappingProfile(Assembly assembly)
            => ApplyMappingsFromAssembly(assembly);

        //происходит сканирование типов, экспортированных из указанной сборки (assembly)
        //, чтобы найти типы, которые реализуют интерфейс IMapWith<>.
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition()
                == typeof(IMapWith<>))).ToList();

            foreach (var type in types)
            {
                //Для каждого найденного типа, реализующего IMapWith<>,
                //создается экземпляр этого типа с помощью Activator.CreateInstance.
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping");

                //выполняет вызов указанного метода (Mapping) для объекта instance. 
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
