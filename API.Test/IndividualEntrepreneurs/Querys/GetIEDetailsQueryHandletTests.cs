using API.Application.Common.Exceptions;
using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Application.IndividualEntrepreneurs.Queries.GetIEDetails;
using API.DAL;
using API.Test.Common;
using AutoMapper;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.IndividualEntrepreneurs.Querys
{
    [Collection("QueryCollection")]
    public class GetIEDetailsQueryHandletTests
    {
        private readonly ApiDbContext Context;
        private readonly IMapper Mapper;

        public GetIEDetailsQueryHandletTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetIEDetailsQueryHandler_Success()
        {
            // Arrange
            var handler = new GetIEDetailsQueryHandler(Context, Mapper);
            var handlerCreteIE = new CreateIECommandHandler(Context);

            var IEId = await handlerCreteIE.Handle(
                new CreateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderId = EntityContextFactory.FounderA.Id
                },
                CancellationToken.None);

            // Act
            var result = await handler.Handle(
                new GetIEDetailsQuery
                {
                    Id = IEId
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<IEDetailsVm>();
            result.INN.ShouldBe(EntityContextFactory.IndividualEntrepreneurA.INN);
            result.Founder.Id.ShouldBe(EntityContextFactory.FounderA.Id);
        }

        [Fact]
        public async Task GetIEDetailsQueryHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new GetIEDetailsQueryHandler(Context, Mapper);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new GetIEDetailsQuery
                    {
                        Id = Guid.NewGuid()
                    }, CancellationToken.None));
        }
    }
}
