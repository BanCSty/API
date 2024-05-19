using API.Application.Common.Exceptions;
using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Application.IndividualEntrepreneurs.Queries.GetIEDetails;
using API.DAL;
using API.DAL.Interfaces;
using API.Domain;
using API.Test.Common;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.IndividualEntrepreneurs.Querys
{
    [Collection("QueryCollection")]
    public class GetIEDetailsQueryHandlerTests
    {
        private readonly ApiDbContext Context;
        private readonly IBaseRepository<IndividualEntrepreneur> _IERepository;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetIEDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            _IERepository = fixture.IndividualEntrepreneurRepository;
            _founderRepository = fixture.FounderRepository;
            _unitOfWork = fixture.UnitOfWork;
        }

        [Fact]
        public async Task GetIEDetailsQueryHandler_Success()
        {
            // Arrange
            var handler = new GetIEDetailsQueryHandler(_IERepository);
            var handlerCreteIE = new CreateIECommandHandler(_IERepository, _founderRepository, _unitOfWork);

            await handlerCreteIE.Handle(
                new CreateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderINN = EntityContextFactory.FounderA.INN
                },
                CancellationToken.None);

            // Act
            var result = await handler.Handle(
                new GetIEDetailsQuery
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<IEDetailsVm>();
            result.INN.ShouldBe(EntityContextFactory.IndividualEntrepreneurA.INN);
        }

        [Fact]
        public async Task GetIEDetailsQueryHandler_FailOnWrongINN()
        {
            // Arrange
            var handler = new GetIEDetailsQueryHandler(_IERepository);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new GetIEDetailsQuery
                    {
                        INN = "123456789108"
                    }, CancellationToken.None));
        }
    }
}
