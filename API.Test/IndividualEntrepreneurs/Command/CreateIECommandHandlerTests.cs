using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using API.Test;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using API.Application.Founders.Queries.GetFoundDetails;

namespace API.Test.IndividualEntrepreneurs.Command
{
    public class CreateIECommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateIECommandHandler_Success()
        {
            // Arrange - подготовка данных для теста
            var handler = new CreateIECommandHandler(Context);

            // Act - выполнение логики
            var IEId = await handler.Handle(
                new CreateIECommand
                {
                    INN = EntityContextFactory.IndividualEntrepreneurA.INN,
                    Name = EntityContextFactory.IndividualEntrepreneurA.Name,
                    FounderId = EntityContextFactory.FounderA.Id
                },
                CancellationToken.None);

            // Assert - проверка результата
            Assert.NotNull(
                await Context.IndividualEntrepreneurs.SingleOrDefaultAsync(ie =>
                    ie.INN == EntityContextFactory.IndividualEntrepreneurA.INN 
                    && ie.Id == IEId));

            //Проверяем, добавилась ли сущность ИП к учредителю
            Assert.NotNull(
                await Context.Founders.SingleOrDefaultAsync(f => 
                f.IndividualEntrepreneur.Id == IEId &&
                f.Id == EntityContextFactory.FounderA.Id));
        }
    }
}
