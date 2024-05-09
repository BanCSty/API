using API.Application.Common.Exceptions;
using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Application.LegalEntitys.Queries.GetLegalEntityDetails;
using API.DAL;
using API.Test.Common;
using AutoMapper;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.LegalEntitys.Querys
{
    [Collection("QueryCollection")]
    public class GetLegalEntityDetailsQueryHandlerTests
    {
        private readonly ApiDbContext Context;
        private readonly IMapper Mapper;

        public GetLegalEntityDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetLegalEntityDetailsQueryHandler_Success()
        {
            // Arrange
            var handler = new GetLegalEntityDetailsQueryHandler (Context, Mapper);
            var handlerCreteLE = new CreateLegalEntityCommandHandler(Context);

            var LEId = await handlerCreteLE.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderIds = new List<Guid> { EntityContextFactory.FounderA.Id}
                },
                CancellationToken.None);

            // Act
            var result = await handler.Handle(
                new GetLegalEntityDetailsQuery
                {
                    Id = LEId
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<LegalEntityDetailsVm>();
            result.INN.ShouldBe(EntityContextFactory.IndividualEntrepreneurA.INN);
            result.Founders.Count().ShouldBe(1);
        }

        [Fact]
        public async Task GetLegalEntityDetailsQueryHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new GetLegalEntityDetailsQueryHandler(Context, Mapper);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new GetLegalEntityDetailsQuery
                    {
                        Id = Guid.NewGuid()
                    }, CancellationToken.None));
        }
    }
}
