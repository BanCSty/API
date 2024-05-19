using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Application.LegalEntitys.Queries.GetLegalEntityList;
using API.DAL;
using API.DAL.Interfaces;
using API.Domain;
using API.Test.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.LegalEntitys.Querys
{
    [Collection("QueryCollection")]
    public class GetLegalEntityListQueryHandlerTests
    {
        private readonly ApiDbContext Context;
        private readonly IBaseRepository<LegalEntity> _legalEntityRepository;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetLegalEntityListQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            _legalEntityRepository = fixture.LegalEntityRepository;
            _founderRepository = fixture.FounderRepository;
            _unitOfWork = fixture.UnitOfWork;
        }

        [Fact]
        public async Task GetIEListQueryHandler_Success()
        {
            // Arrange
            var handler = new GetLegalEntityListQueryHandler(_legalEntityRepository);
            var handlerCreteLE = new CreateLegalEntityCommandHandler(_legalEntityRepository, _founderRepository,_unitOfWork);

            await handlerCreteLE.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurB.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurB.Name,
                    FounderIds = new List<Guid> { EntityContextFactory.FounderB.Id }
                },
                CancellationToken.None);

            // Act
            var result = await handler.Handle(
                new GetLegalEntityListQuery
                {
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<LegalEntityListVm>();
            //Две записи т.к мы создали в GetLegalEntityDetailsQueryHandletTests еще одно Юр лицо
            result.LegalEntitys.Count.ShouldBe(2);
        }
    }
}
