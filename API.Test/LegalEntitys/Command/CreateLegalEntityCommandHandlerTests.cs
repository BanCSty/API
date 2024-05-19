using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var handler = new CreateLegalEntityCommandHandler(LegalEntityRepository, FounderRepository, UnitOfWork);

            // Act - выполнение логики
            await handler.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.LegalEntityA.INN,
                    Name = EntityContextFactory.LegalEntityA.Name,
                    FounderINNs = new List<string> {EntityContextFactory.FounderA.INN, EntityContextFactory.FounderB.INN }
                },
                CancellationToken.None);

            // Assert - проверка результата
            Assert.NotNull(
                await Context.LegalEntitys.SingleOrDefaultAsync(le =>
                    le.INN == EntityContextFactory.LegalEntityA.INN
                    && le.Name == EntityContextFactory.LegalEntityA.Name));

            // Получение учредителя из базы данных
            var retrievedFounder = await Context.Founders
                .Include(f => f.LegalEntities)
                .FirstOrDefaultAsync(f => f.INN == EntityContextFactory.FounderA.INN);

            var legalEntity = await Context.LegalEntitys.FirstOrDefaultAsync(le => le.INN == EntityContextFactory.LegalEntityA.INN);

            //Проверяем, добавилась ли сущность юр лица к учредителю
            Assert.Contains(legalEntity, retrievedFounder.LegalEntities);
        }

        [Fact]
        public async Task CreateLegalEntityCommandHandler_FailOnAlreadyUsedINN()
        {
            // Arrange - подготовка данных для теста
            var handler = new CreateLegalEntityCommandHandler(LegalEntityRepository, FounderRepository, UnitOfWork);

            // Act - выполнение логики
            await handler.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.LegalEntityA.INN,
                    Name = EntityContextFactory.LegalEntityA.Name,
                    FounderINNs = new List<string> { EntityContextFactory.FounderA.INN, EntityContextFactory.FounderB.INN }
                },
                CancellationToken.None);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(
                    new CreateLegalEntityCommand
                    {
                        INN = EntityContextFactory.LegalEntityA.INN,
                        Name = EntityContextFactory.LegalEntityA.Name,  
                        FounderINNs = new List<string> { EntityContextFactory.FounderA.INN, EntityContextFactory.FounderB.INN }
                    },
                    CancellationToken.None));
        }
    }
}
