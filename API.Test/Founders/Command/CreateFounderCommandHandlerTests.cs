using API.Application.Founders.Command.CreateFounder;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.Founders.Command
{
    public class CreateFounderCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateFounderCommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new CreateFounderCommandHandler(Context);
            var inn = 123456789103;
            var firstName = "Bob";
            var lastName = "Tromb";
            var middleName = "Sorken";

            // Act - выполнение логики
            var founderId = await handler.Handle(
                new CreateFounderCommand
                {
                    INN = inn,
                    FirstName = firstName,
                    LastName = lastName,
                    MiddleName = middleName
                },
                CancellationToken.None);

            // Assert - проверка результата
            Assert.NotNull(
                await Context.Founders.SingleOrDefaultAsync(founder =>
                    founder.INN == inn && founder.FirstName == firstName &&
                    founder.LastName == lastName && founder.MiddleName == middleName &&
                    founder.Id == founderId));
        }
    }
}
