using API.Application.Common.Exceptions;
using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Domain;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.LegalEntitys.Command
{
    public class CreateLegalEntityCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateLegalEntityCommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new CreateLegalEntityCommandHandler(Context);

            // Act - выполнение логики
            var LegalEntityId = await handler.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.LegalEntityA.INN,
                    Name = EntityContextFactory.LegalEntityA.Name,
                    FounderIds = new List<Guid> {EntityContextFactory.FounderA.Id, EntityContextFactory.FounderB.Id }
                },
                CancellationToken.None);

            // Assert - проверка результата
            Assert.NotNull(
                await Context.LegalEntitys.SingleOrDefaultAsync(le =>
                    le.INN == EntityContextFactory.LegalEntityA.INN
                    && le.Id == LegalEntityId));

            // Получение учредителя из базы данных
            var retrievedFounder = await Context.Founders
                .Include(f => f.LegalEntities)
                .FirstOrDefaultAsync(f => f.Id == EntityContextFactory.FounderA.Id);

            var legalEntity = await Context.LegalEntitys.FirstOrDefaultAsync(le => le.Id == LegalEntityId);

            //Проверяем, добавилась ли сущность юр лица к учредителю
            Assert.Contains(legalEntity, retrievedFounder.LegalEntities);
        }

        [Fact]
        public async Task CreateLegalEntityCommandHandler_FailOnAlreadyUsedINN()
        {
            // Arrange - подготовка данных для теста
            var handler = new CreateLegalEntityCommandHandler(Context);

            // Act - выполнение логики
            var LegalEntityId = await handler.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.LegalEntityA.INN,
                    Name = EntityContextFactory.LegalEntityA.Name,
                    FounderIds = new List<Guid> { EntityContextFactory.FounderA.Id, EntityContextFactory.FounderB.Id }
                },
                CancellationToken.None);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(
                    new CreateLegalEntityCommand
                    {
                        INN = EntityContextFactory.LegalEntityA.INN,
                        Name = EntityContextFactory.LegalEntityA.Name,
                        FounderIds = new List<Guid> { EntityContextFactory.FounderA.Id, EntityContextFactory.FounderB.Id }
                    },
                    CancellationToken.None));
        }
    }
}
