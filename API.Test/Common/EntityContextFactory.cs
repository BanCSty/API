using API.DAL;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Test.Common
{
    public static class EntityContextFactory
    {
        public static Founder FounderA = new Founder
        {
            Id = Guid.Parse("b0e6cbae-68f3-4001-bcdc-5ce3f5114308"),
            INN = "123456789101",
            FirstName = "Alex",
            LastName = "Bodui",
            MiddleName = "Fring",
            DateCreate = DateTime.Now,
            DateUpdate = null,
        };

        public static Founder FounderB = new Founder
        {
            Id = Guid.Parse("ce83865b-c636-42ec-9774-60398792dcbe"),
            INN = "123456789102",
            FirstName = "Ban",
            LastName = "Broud",
            MiddleName = "Flintin",
            DateCreate = DateTime.Now,
            DateUpdate = null,
        };

        public static IndividualEntrepreneur IndividualEntrepreneurA = new IndividualEntrepreneur
        {
            Id = Guid.Parse("de83865b-c636-42ec-9774-60398792dcbe"),
            Name = "IE Streem",
            INN = "225252525222",
            DateCreate = DateTime.Now,
            DateUpdate = null,
            FounderId = FounderA.Id,
            Founder = FounderA
        };
        public static IndividualEntrepreneur IndividualEntrepreneurB = new IndividualEntrepreneur
        {
            Id = Guid.Parse("ee83865b-c636-42ec-9774-60398792dcbe"),
            INN = "325252525222",
            Name = "IE Volga",
            DateCreate = DateTime.Now,
            DateUpdate = null,
            FounderId = FounderB.Id,
            Founder = FounderB
        };

        public static LegalEntity LegalEntityA = new LegalEntity
        {
            Id = Guid.NewGuid(),
            INN = "425252525222",
            Name = "OOO Doom",
            DateCreate = DateTime.Now,
            DateUpdate = null,
            Founders = new List<Founder> { FounderA }
        };

        public static LegalEntity LegalEntityB = new LegalEntity
        {
            Id = Guid.NewGuid(),
            INN = "525252525222",
            Name = "OOO Latron",
            DateCreate = DateTime.Now,
            DateUpdate = null,
            Founders = new List<Founder> { FounderA, FounderB }
        };

        public static ApiDbContext Create()
        {
           
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApiDbContext(options);
            context.Database.EnsureCreated(); //Убедимся что база создана
            //Добавляем учредителей
            context.Founders.AddRange(
                new Founder
                {
                    Id = FounderA.Id,
                    INN = FounderA.INN,
                    FirstName = FounderA.FirstName,
                    LastName = FounderA.LastName,
                    MiddleName = FounderA.MiddleName,
                    DateCreate = FounderA.DateCreate,
                    DateUpdate = FounderA.DateUpdate,
                },
                new Founder
                {
                    Id = FounderB.Id,
                    INN = FounderB.INN,
                    FirstName = FounderB.FirstName,
                    LastName = FounderB.LastName,
                    MiddleName = FounderB.MiddleName,
                    DateCreate = FounderB.DateCreate,
                    DateUpdate = FounderB.DateUpdate,
                }
            );

            context.SaveChanges();
            return context;
        }

        public static void Destroy(ApiDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
