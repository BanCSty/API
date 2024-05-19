using API.Application.Common.Exceptions;
using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Application.LegalEntitys.Command.UpdateLegalEntity;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.LegalEntitys.Command
{
    public class UpdateLegalEntityCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateLegalEntityCommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new UpdateLegalEntityCommandHandler(LegalEntityRepository, FounderRepository, UnitOfWork);

            var handlerCreate = new CreateLegalEntityCommandHandler(LegalEntityRepository, FounderRepository, UnitOfWork);

            await handlerCreate.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.LegalEntityA.INN,
                    Name = EntityContextFactory.LegalEntityA.Name,
                    FounderINNs = new List<string> { EntityContextFactory.FounderA.INN, EntityContextFactory.FounderB.INN }
                },
                CancellationToken.None);

            // Act - выполнение логики
            await handler.Handle(new UpdateLegalEntityCommand
            {
                INN = EntityContextFactory.LegalEntityA.INN,
                Name = EntityContextFactory.LegalEntityB.Name,
                FounderINNs = new List<string> { EntityContextFactory.FounderB.INN}

            }, CancellationToken.None);

            //Обновилась ли сущность Юр лица
            Assert.NotNull(Context.LegalEntitys.SingleOrDefault(LE =>
                LE.Name == EntityContextFactory.LegalEntityB.Name &&
                LE.INN == EntityContextFactory.LegalEntityA.INN));

            // Получение учредителя из базы данных
            var retrievedFounder = await Context.Founders
                .Include(f => f.LegalEntities)
                .FirstOrDefaultAsync(f => f.INN == EntityContextFactory.FounderB.INN);

            var legalEntity = await Context.LegalEntitys.FirstOrDefaultAsync(le => le.INN == EntityContextFactory.LegalEntityA.INN);

            //Проверяем, удалилась ли сущность юр лица из учредителя
            Assert.Contains(legalEntity, retrievedFounder.LegalEntities);
        }

        [Fact]
        public async Task UpdateLegalEntityCommandHandler_FailOnWrongINN()
        {
            // Arrange
            var handler = new UpdateLegalEntityCommandHandler(LegalEntityRepository, FounderRepository, UnitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateLegalEntityCommand
                    {
                        INN = "123456789108", //fail
                        Name = EntityContextFactory.LegalEntityB.Name,
                        FounderINNs = new List<string> { EntityContextFactory.FounderB.INN }
                    },
                    CancellationToken.None));
        }
    }
}
