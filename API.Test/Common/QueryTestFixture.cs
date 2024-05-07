using API.Application.Common.Mappings;
using API.Application.Interfaces;
using API.DAL;
using AutoMapper;
using System;
using Xunit;

namespace API.Test.Common
{
    public class QueryTestFixture : IDisposable
    {
        public ApiDbContext Context;
        public IMapper Mapper;

        public QueryTestFixture()
        {
            Context = EntityContextFactory.Create();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(
                    typeof(IApiDbContext).Assembly));
            });
            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            EntityContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
