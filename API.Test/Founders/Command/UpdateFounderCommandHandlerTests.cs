using API.Application.Common.Exceptions;
using API.Application.Founders.Command.UpdateFounder;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.Test.Founders.Command
{
    public class UpdateFounderCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandHandler_Success()
        {
            // Arrange
            var handler = new UpdateFounderCommandHandler(Context);
            var updatedFirstName = "Mamba";

            // Act
            await handler.Handle(new UpdateFounderCommand
            {
                Id = EntityContextFactory.FounderA.Id,
                FirstName = updatedFirstName,
                LastName = EntityContextFactory.FounderA.LastName,
                MiddleName = EntityContextFactory.FounderA.MiddleName,
                INN = EntityContextFactory.FounderA.INN,              
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(await Context.Founders.SingleOrDefaultAsync(founder =>
                founder.Id == EntityContextFactory.FounderA.Id &&
                founder.FirstName == updatedFirstName));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new UpdateFounderCommandHandler(Context);

            // Act

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateFounderCommand
                    {
                        Id = Guid.NewGuid(),
                        INN = 11111111,
                        FirstName = "fail",
                        LastName = "fail",
                        MiddleName = "fail"
                    },
                    CancellationToken.None));
        }

        [Fact]
        public async Task UpdateFounderCommandHandler_FailOnINNAlreadyExtist()
        {
            // Arrange - подготовка данных для теста
            var handler = new UpdateFounderCommandHandler(Context);
            var id = Guid.Parse("b0e6cbae-68f3-4001-bcdc-5ce3f5114308");
            var inn = 123456789102;
            var firstName = "Bob";
            var lastName = "Tromb";
            var middleName = "Sorken";


            // Assert - проверка результата
            //Выбросится исключение, указывающее на уже существующего учредителя с таким ИНН
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(

                    new UpdateFounderCommand
                    {
                        Id = id,
                        INN = inn,
                        FirstName = firstName,
                        LastName = lastName,
                        MiddleName = middleName
                    },
                CancellationToken.None));
        }
    }
}
