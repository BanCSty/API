using API.Application.Common.Exceptions;
using API.Application.Founders.Command.DeleteFounder;
using API.Test.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.Founders.Command
{
    public class DeleteFounderCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteFounderCommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new DeleteFounderCommandHandler(FounderRepository, LegalEntityRepository, 
                IndividualEntrepreneurRepository, UnitOfWork);

            // Act - выполнение логики
            await handler.Handle(new DeleteFounderCommand
            {
                INN = EntityContextFactory.FounderA.INN,

            }, CancellationToken.None);

            // Assert - проверка результата
            Assert.Null(Context.Founders.SingleOrDefault(founder =>
                founder.INN == EntityContextFactory.FounderA.INN));
        }

        [Fact]
        public async Task DeleteFounderCommandHandler_FailOnWrongINN()
        {
            // Arrange
            var handler = new DeleteFounderCommandHandler(FounderRepository, LegalEntityRepository, 
                IndividualEntrepreneurRepository, UnitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteFounderCommand
                    {
                        INN = "123456789108"
                    },
                    CancellationToken.None));
        }
    }
}
