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
    }
}
