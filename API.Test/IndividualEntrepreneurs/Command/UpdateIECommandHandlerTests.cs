using API.Application.Common.Exceptions;
using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Application.IndividualEntrepreneurs.Command.UpdateIE;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
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

            var updateName = "IE Shorud";
            var updateFounderINN = EntityContextFactory.FounderB.INN;

            // Act - выполнение логики
            await handlerCreate.Handle(
                new CreateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderINN = EntityContextFactory.FounderA.INN
                },
                CancellationToken.None);

            await handler.Handle(
                new UpdateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = updateName,
                    FounderINN = updateFounderINN
                },
                CancellationToken.None);

            // Assert - проверка результата
            Assert.NotNull(
                await Context.IndividualEntrepreneurs.SingleOrDefaultAsync(ie =>
                    ie.Name == updateName &&
                    ie.INN == EntityContextFactory.IndividualEntrepreneurA.INN));

            //Проверяем, добавилась ли сущность ИП к учредителю
            Assert.NotNull(
                await Context.Founders.SingleOrDefaultAsync(f =>
                f.IndividualEntrepreneur.INN == EntityContextFactory.IndividualEntrepreneurA.INN 
                && f.INN == EntityContextFactory.FounderB.INN));
        }

        [Fact]
        public async Task UpdateIECommandHandler_FailOnWrongINN()
        {
            // Arrange
            var handler = new UpdateIECommandHandler(IndividualEntrepreneurRepository, FounderRepository, UnitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateIECommand
                    {
                        Name = "Fail",
                        INN = "123456789108",
                        FounderINN = "123456789102"
                    },
                    CancellationToken.None));
        }
    }
}
