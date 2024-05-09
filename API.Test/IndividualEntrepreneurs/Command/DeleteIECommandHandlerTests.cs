using API.Application.Common.Exceptions;
using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Application.IndividualEntrepreneurs.Command.DeleteIE;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.IndividualEntrepreneurs.Command
{
    public class DeleteIECommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteIECommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new DeleteIECommandHandler(Context);

            var handlerCreate = new CreateIECommandHandler(Context);

            // Act - выполнение логики
            var IEId = await handlerCreate.Handle(
                new CreateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderId = EntityContextFactory.FounderA.Id
                },
                CancellationToken.None);

            await handler.Handle(new DeleteIECommand
            {
                Id = IEId,

            }, CancellationToken.None);

            Assert.Null(Context.IndividualEntrepreneurs.SingleOrDefault(Ie =>
                Ie.Id == EntityContextFactory.FounderA.Id));

            //Удалилась ли сущность ИП из учредителя
            Assert.Null(Context.Founders.SingleOrDefault(founders =>
                founders.IndividualEntrepreneur.Id == IEId));
        }

        [Fact]
        public async Task DeleteIECommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new DeleteIECommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteIECommand
                    {
                        Id = Guid.NewGuid()
                    },
                    CancellationToken.None));
        }
    }
}
