using API.Application.Common.Exceptions;
using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Application.IndividualEntrepreneurs.Command.DeleteIE;
using API.Test.Common;
using System.Linq;
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
            var handler = new DeleteIECommandHandler(IndividualEntrepreneurRepository, FounderRepository, UnitOfWork);

            var handlerCreate = new CreateIECommandHandler(IndividualEntrepreneurRepository, FounderRepository, UnitOfWork);

            // Act - выполнение логики
            await handlerCreate.Handle(
                new CreateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderINN = EntityContextFactory.FounderA.INN
                },
                CancellationToken.None);

            await handler.Handle(new DeleteIECommand
            {
                INN = EntityContextFactory.IndividualEntrepreneurA.INN,

            }, CancellationToken.None);

            //Удалилось ли ИП
            Assert.Null(Context.IndividualEntrepreneurs.SingleOrDefault(Ie =>
                Ie.INN == EntityContextFactory.IndividualEntrepreneurA.INN));

            //Удалилась ли сущность ИП из учредителя
            Assert.Null(Context.Founders.SingleOrDefault(founders =>
                founders.IndividualEntrepreneur.INN == EntityContextFactory.IndividualEntrepreneurA.INN));
        }

        [Fact]
        public async Task DeleteIECommandHandler_FailOnWrongINN()
        {
            // Arrange
            var handler = new DeleteIECommandHandler(IndividualEntrepreneurRepository, FounderRepository, UnitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteIECommand
                    {
                        INN = "123456789108"
                    },
                    CancellationToken.None));
        }
    }
}
