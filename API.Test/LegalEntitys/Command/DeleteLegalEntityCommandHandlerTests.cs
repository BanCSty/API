using API.Application.Common.Exceptions;
using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Application.LegalEntitys.Command.DeleteLegalEntity;
using API.Domain;
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
    public class DeleteLegalEntityCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteLegalEntityCommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new DeleteLegalEntityCommandHandler(LegalEntityRepository, FounderRepository, UnitOfWork);

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
            await handler.Handle(new DeleteLegalEntityCommand
            {
                INN = EntityContextFactory.LegalEntityA.INN,

            }, CancellationToken.None);

            Assert.Null(Context.LegalEntitys.SingleOrDefault(LE =>
                LE.INN == EntityContextFactory.LegalEntityA.INN));

            // Получение учредителя из базы данных
            var retrievedFounder = await Context.Founders
                .Include(f => f.LegalEntities)
                .FirstOrDefaultAsync(f => f.INN == EntityContextFactory.FounderA.INN);

            var legalEntity = new LegalEntity
            (
                EntityContextFactory.LegalEntityA.INN,
                EntityContextFactory.LegalEntityA.Name,
                DateTime.Now,
                new List<Founder> { EntityContextFactory.FounderA, EntityContextFactory.FounderB }
            );

            //Проверяем, удалилась ли сущность юр лица из учредителя
            Assert.DoesNotContain(legalEntity, retrievedFounder.LegalEntities);
        }

        [Fact]
        public async Task DeleteLegalEntityCommandHandler_FailOnWrongINN()
        {
            // Arrange
            var handler = new DeleteLegalEntityCommandHandler(LegalEntityRepository, FounderRepository, UnitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteLegalEntityCommand
                    {
                        INN = "123456789108"
                    },
                    CancellationToken.None));
        }
    }
}
