using API.Application.Founders.Queries.GetFounderList;
using API.DAL;
using API.DAL.Interfaces;
using API.Domain;
using API.Test.Common;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.Founders.Querys
{
    [Collection("QueryCollection")]
    public class GetFounderListQueryHandlerTests
    {
        private readonly ApiDbContext Context;
        private readonly IBaseRepository<Founder> _founderRepository;

        public GetFounderListQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            _founderRepository = fixture.FounderRepository;
        }

        [Fact]
        public async Task GetFounderListQueryHandler_Success()
        {
            // Arrange
            var handler = new GetFounderListQueryHandler(_founderRepository);

            // Act
            var result = await handler.Handle(
                new GetFounderListQuery
                {
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<FounderListVm>();
            result.Founders.Count.ShouldBe(2);
        }
    }
}
