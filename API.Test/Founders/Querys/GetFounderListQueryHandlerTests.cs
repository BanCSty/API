using API.Application.Founders.Queries.GetFounderList;
using API.DAL;
using API.Test.Common;
using AutoMapper;
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
        private readonly IMapper Mapper;

        public GetFounderListQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetNoteListQueryHandler_Success()
        {
            // Arrange
            var handler = new GetFounderListQueryHandler(Context, Mapper);

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
