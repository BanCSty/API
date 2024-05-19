using API.Application.Founders.Command.CreateFounder;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
using System;
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
            var handler = new CreateFounderCommandHandler(FounderRepository);
            var inn = "123456789103";
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
                    founder.INN == inn && founder.FullName.FirstName == firstName &&
                    founder.FullName.LastName == lastName && founder.FullName.MiddleName == middleName &&
                    founder.INN == inn));
        }

        [Fact]
        public async Task CreateFounderCommandHandler_FailOnINNAlreadyExtist()
        {
            // Arrange - подготовка данных для теста
            var handler = new CreateFounderCommandHandler(FounderRepository);
            var inn = "123456789101";
            var firstName = "Bob";
            var lastName = "Tromb";
            var middleName = "Sorken";


            // Assert - проверка результата
            //Выбросится исключение, указывающее на уже существующего учредителя с таким ИНН
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(

                    new CreateFounderCommand
                    {
                        INN = inn,
                        FirstName = firstName,
                        LastName = lastName,
                        MiddleName = middleName
                    },
                CancellationToken.None));
        }
    }
}
