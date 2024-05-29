using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Test.Common;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace API.Test.IndividualEntrepreneurs.Command
{
    public class CreateIECommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateIECommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new CreateIECommandHandler(IndividualEntrepreneurRepository, FounderRepository, UnitOfWork);

            // Act - выполнение логики
                await handler.Handle(
                new CreateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderINN = EntityContextFactory.FounderA.INN.Value
                },
                CancellationToken.None);

            // Assert - проверка результата
            Assert.NotNull(
                await Context.IndividualEntrepreneurs.SingleOrDefaultAsync(ie =>
                    ie.INN == EntityContextFactory.IndividualEntrepreneurA.INN));

            //Проверяем, добавилась ли сущность ИП к учредителю
            Assert.NotNull(
                await Context.Founders.SingleOrDefaultAsync(f => 
                f.IndividualEntrepreneur.INN == EntityContextFactory.IndividualEntrepreneurA.INN));
        }
    }
}
