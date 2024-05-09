using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Application.IndividualEntrepreneurs.Queries.GetIEList;
using API.DAL;
using API.Test.Common;
using AutoMapper;
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
        private readonly IMapper Mapper;

        public GetIEListQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetIEListQueryHandler_Success()
        {
            // Arrange
            var handler = new GetIEListQueryHandler(Context, Mapper);
            var handlerCreteIE = new CreateIECommandHandler(Context);

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
