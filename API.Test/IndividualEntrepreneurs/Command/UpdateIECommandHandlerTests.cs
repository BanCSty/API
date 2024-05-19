using API.Application.Common.Exceptions;
using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Application.IndividualEntrepreneurs.Command.UpdateIE;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.IndividualEntrepreneurs.Command
{
    public class UpdateIECommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateIECommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new UpdateIECommandHandler(IndividualEntrepreneurRepository, FounderRepository, UnitOfWork);
            var handlerCreate = new CreateIECommandHandler(IndividualEntrepreneurRepository, FounderRepository, UnitOfWork);

            var updateINN = "111111111111";
            var updateName = "IE Shorud";
            var updateFounderId = EntityContextFactory.FounderB.Id;

            // Act - выполнение логики
            var IEId = await handlerCreate.Handle(
                new CreateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderId = EntityContextFactory.FounderA.Id
                },
                CancellationToken.None);

            await handler.Handle(
                new UpdateIECommand
                {
                    Id = IEId,
                    INN = updateINN,
                    Name = updateName,
                    FounderId = updateFounderId
                },
                CancellationToken.None);

            // Assert - проверка результата
            Assert.NotNull(
                await Context.IndividualEntrepreneurs.SingleOrDefaultAsync(ie =>
                    ie.Id == IEId &&
                    ie.Name == updateName &&
                    ie.FounderId == updateFounderId &&
                    ie.INN == updateINN));

            //Проверяем, добавилась ли сущность ИП к учредителю
            Assert.NotNull(
                await Context.Founders.SingleOrDefaultAsync(f =>
                f.IndividualEntrepreneur.Id == IEId &&
                f.Id == updateFounderId));
        }

        [Fact]
        public async Task UpdateIECommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new UpdateIECommandHandler(IndividualEntrepreneurRepository, FounderRepository, UnitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateIECommand
                    {
                        Id = Guid.NewGuid(),
                        Name = "Fail",
                        INN = "911",
                        FounderId = Guid.NewGuid()
                    },
                    CancellationToken.None));
        }
    }
}
