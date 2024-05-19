using API.Application.Common.Exceptions;
using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Application.LegalEntitys.Queries.GetLegalEntityDetails;
using API.DAL;
using API.DAL.Interfaces;
using API.Domain;
using API.Test.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.LegalEntitys.Querys
{
    [Collection("QueryCollection")]
    public class GetLegalEntityDetailsQueryHandlerTests
    {
        private readonly ApiDbContext Context;
        private readonly IBaseRepository<LegalEntity> _legalEntityRepository;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetLegalEntityDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            _legalEntityRepository = fixture.LegalEntityRepository;
            _founderRepository = fixture.FounderRepository;
            _unitOfWork = fixture.UnitOfWork;
        }

        [Fact]
        public async Task GetLegalEntityDetailsQueryHandler_Success()
        {
            // Arrange
            var handler = new GetLegalEntityDetailsQueryHandler(_legalEntityRepository);
            var handlerCreteLE = new CreateLegalEntityCommandHandler(_legalEntityRepository, _founderRepository, _unitOfWork);

            await handlerCreteLE.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderINNs = new List<string> { EntityContextFactory.FounderA.INN}
                },
                CancellationToken.None);

            // Act
            var result = await handler.Handle(
                new GetLegalEntityDetailsQuery
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<LegalEntityDetailsVm>();
            result.INN.ShouldBe(EntityContextFactory.IndividualEntrepreneurA.INN);
            result.Founders.Count().ShouldBe(1);
        }

        [Fact]
        public async Task GetLegalEntityDetailsQueryHandler_FailOnWrongINN()
        {
            // Arrange
            var handler = new GetLegalEntityDetailsQueryHandler(_legalEntityRepository);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new GetLegalEntityDetailsQuery
                    {
                        INN = "123456789108"
                    }, CancellationToken.None));
        }
    }
}
