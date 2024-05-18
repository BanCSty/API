using API.DAL;
using API.DAL.Interfaces;
using API.DAL.Repositories;
using API.Domain;
using System;
using Xunit;

namespace API.Test.Common
{
    public class QueryTestFixture : IDisposable
    {
        public ApiDbContext Context;
        public readonly IUnitOfWork UnitOfWork;
        public readonly IBaseRepository<Founder> FounderRepository;
        public readonly IBaseRepository<LegalEntity> LegalEntityRepository;
        public readonly IBaseRepository<IndividualEntrepreneur> IndividualEntrepreneurRepository;

        public QueryTestFixture()
        {
            Context = EntityContextFactory.Create();
            UnitOfWork = new UnitOfWork(Context);
            FounderRepository = new FounderRepository(Context);
            LegalEntityRepository = new LegalEntityRepository(Context);
            IndividualEntrepreneurRepository = new IndividualEntrepreneurRepository(Context);
        }

        public void Dispose()
        {
            EntityContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
