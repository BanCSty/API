using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Application.LegalEntitys.Queries.GetLegalEntityList;
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
    public class GetLegalEntityListQueryHandlerTests
    {
        private readonly ApiDbContext Context;
        private readonly IMapper Mapper;

        public GetLegalEntityListQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetIEListQueryHandler_Success()
        {
            // Arrange
            var handler = new GetLegalEntityListQueryHandler(Context, Mapper);
            var handlerCreteLE = new CreateLegalEntityCommandHandler(Context);

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
