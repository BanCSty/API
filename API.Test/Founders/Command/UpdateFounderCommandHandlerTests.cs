﻿using API.Application.Common.Exceptions;
using API.Application.Founders.Command.UpdateFounder;
using API.Test.Common;
using Microsoft.EntityFrameworkCore;
using System;
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
            var handler = new UpdateFounderCommandHandler(FounderRepository, LegalEntityRepository, 
                IndividualEntrepreneurRepository, UnitOfWork);
            var updatedFirstName = "Mamba";

            // Act
            await handler.Handle(new UpdateFounderCommand
            {
                FirstName = updatedFirstName,
                LastName = EntityContextFactory.FounderA.FullName.LastName,
                MiddleName = EntityContextFactory.FounderA.FullName.MiddleName,
                INN = EntityContextFactory.FounderA.INN,              
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(await Context.Founders.SingleOrDefaultAsync(founder =>
                founder.INN == EntityContextFactory.FounderA.INN &&
                founder.FullName.FirstName == updatedFirstName));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongINN()
        {
            // Arrange
            var handler = new UpdateFounderCommandHandler(FounderRepository, LegalEntityRepository, 
                IndividualEntrepreneurRepository, UnitOfWork);

            // Act

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateFounderCommand
                    {
                        INN = "123456789108",
                        FirstName = "fail",
                        LastName = "fail",
                        MiddleName = "fail"
                    },
                    CancellationToken.None));
        }
    }
}
