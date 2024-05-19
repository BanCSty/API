using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Application.IndividualEntrepreneurs.Queries.GetIEList;
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
    public class GetIEListQueryHandlerTests
    {
        private readonly ApiDbContext Context;
        private readonly IBaseRepository<IndividualEntrepreneur> _IERepository;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetIEListQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            _IERepository = fixture.IndividualEntrepreneurRepository;
            _founderRepository = fixture.FounderRepository;
            _unitOfWork = fixture.UnitOfWork;
        }

        [Fact]
        public async Task GetIEListQueryHandler_Success()
        {
            // Arrange
            var handler = new GetIEListQueryHandler(_IERepository);
            var handlerCreteIE = new CreateIECommandHandler(_IERepository, _founderRepository, _unitOfWork);

            await handlerCreteIE.Handle(
                new CreateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurB.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurB.Name,
                    FounderId = EntityContextFactory.FounderB.Id
                },
                CancellationToken.None);

            // Act
            var result = await handler.Handle(
                new GetIEListQuery
                {
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<IEListVm>();
            //Две записи т.к мы создали в GetIEDetailsQueryHandletTests еще один ИП
            result.IndividualEntrepreneurs.Count.ShouldBe(2);
        }
    }
}
