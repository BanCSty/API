using API.Application.Common.Exceptions;
using API.Application.Founders.Queries.GetFoundDetails;
using API.DAL;
using API.DAL.Interfaces;
using API.Domain;
using API.Test.Common;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.Founders.Querys
{
    [Collection("QueryCollection")]
    public class GetFounderDetailsQueryHandlerTests
    {
        private readonly ApiDbContext Context;
        private readonly IBaseRepository<Founder> _founderRepository;

        public GetFounderDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            _founderRepository = fixture.FounderRepository;
        }

        [Fact]
        public async Task GetFounderDetailsQueryHandler_Success()
        {
            // Arrange
            var handler = new GetFounderDetailsQueryHandler(_founderRepository);

            // Act
            var result = await handler.Handle(
                new GetFounderDetailsQuery
                {
                    Id = EntityContextFactory.FounderA.Id
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<FounderDetailsVm>();
            result.INN.ShouldBe("123456789101");
        }

        [Fact]
        public async Task GetFounderDetailsQueryHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new GetFounderDetailsQueryHandler(_founderRepository);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new GetFounderDetailsQuery
                    {
                        Id = Guid.NewGuid()
                    }, CancellationToken.None));
        }
    }
}
