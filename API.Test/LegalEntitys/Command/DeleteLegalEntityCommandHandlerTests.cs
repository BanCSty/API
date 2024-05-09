using API.Application.Common.Exceptions;
using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Application.LegalEntitys.Command.DeleteLegalEntity;
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
    public class DeleteLegalEntityCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteLegalEntityCommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new DeleteLegalEntityCommandHandler(Context);

            var handlerCreate = new CreateLegalEntityCommandHandler(Context);

            var LEId = await handlerCreate.Handle(
                new CreateLegalEntityCommand
                {
                    INN = EntityContextFactory.LegalEntityA.INN,
                    Name = EntityContextFactory.LegalEntityA.Name,
                    FounderIds = new List<Guid> { EntityContextFactory.FounderA.Id, EntityContextFactory.FounderB.Id }
                },
                CancellationToken.None);

            // Act - выполнение логики
            await handler.Handle(new DeleteLegalEntityCommand
            {
                Id = LEId,

            }, CancellationToken.None);

            Assert.Null(Context.LegalEntitys.SingleOrDefault(Le =>
                Le.Id == EntityContextFactory.LegalEntityA.Id));

            // Получение учредителя из базы данных
            var retrievedFounder = await Context.Founders
                .Include(f => f.LegalEntities)
                .FirstOrDefaultAsync(f => f.Id == EntityContextFactory.FounderA.Id);

            var legalEntity = new LegalEntity
            {
                Id = LEId,
                INN = EntityContextFactory.LegalEntityA.INN,
                Name = EntityContextFactory.LegalEntityA.Name,
                DateCreate = DateTime.Now,
                Founders = new List<Founder> { EntityContextFactory.FounderA, EntityContextFactory.FounderB }
            };

            //Проверяем, удалилась ли сущность юр лица из учредителя
            Assert.DoesNotContain(legalEntity, retrievedFounder.LegalEntities);
        }

        [Fact]
        public async Task DeleteLegalEntityCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new DeleteLegalEntityCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteLegalEntityCommand
                    {
                        Id = Guid.NewGuid()
                    },
                    CancellationToken.None));
        }
    }
}
