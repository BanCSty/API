using API.DAL;
using API.DAL.Interfaces;
using API.DAL.Repositories;
using API.Domain;
using System;

namespace API.Test.Common
{
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly ApiDbContext Context;
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IBaseRepository<Founder> FounderRepository;
        protected readonly IBaseRepository<LegalEntity> LegalEntityRepository;
        protected readonly IBaseRepository<IndividualEntrepreneur> IndividualEntrepreneurRepository;

        public TestCommandBase()
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
}
