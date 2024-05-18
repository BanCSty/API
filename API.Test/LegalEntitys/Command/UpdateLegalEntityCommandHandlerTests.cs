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

            var LEId = await handlerCreate.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.LegalEntityA.INN,
                    Name = EntityContextFactory.LegalEntityA.Name,
                    FounderIds = new List<Guid> { EntityContextFactory.FounderA.Id, EntityContextFactory.FounderB.Id }
                },
                CancellationToken.None);

            // Act - выполнение логики
            await handler.Handle(new UpdateLegalEntityCommand
            {
                Id = LEId,
                INN = EntityContextFactory.LegalEntityB.INN,
                Name = EntityContextFactory.LegalEntityB.Name,
                FounderIds = new List<Guid> { EntityContextFactory.FounderB.Id}

            }, CancellationToken.None);

            //Обновилась ли сущность Юр лица
            Assert.NotNull(Context.LegalEntitys.SingleOrDefault(Le =>
                Le.Id == LEId &&
                Le.INN == EntityContextFactory.LegalEntityB.INN));

            // Получение учредителя из базы данных
            var retrievedFounder = await Context.Founders
                .Include(f => f.LegalEntities)
                .FirstOrDefaultAsync(f => f.Id == EntityContextFactory.FounderB.Id);

            var legalEntity = await Context.LegalEntitys.FirstOrDefaultAsync(le => le.Id == LEId);

            //Проверяем, удалилась ли сущность юр лица из учредителя
            Assert.Contains(legalEntity, retrievedFounder.LegalEntities);
        }

        [Fact]
        public async Task UpdateLegalEntityCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new UpdateLegalEntityCommandHandler(LegalEntityRepository, FounderRepository, UnitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateLegalEntityCommand
                    {
                        Id = Guid.NewGuid(),
                        INN = EntityContextFactory.LegalEntityB.INN,
                        Name = EntityContextFactory.LegalEntityB.Name,
                        FounderIds = new List<Guid> { EntityContextFactory.FounderB.Id }
                    },
                    CancellationToken.None));
        }
    }
}
