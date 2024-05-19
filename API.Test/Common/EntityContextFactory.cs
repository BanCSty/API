using API.DAL;
using API.Domain;
using API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace API.Test.Common
{
    public static class EntityContextFactory
    {
        public static Founder FounderA = new Founder
        (
            (INN)"123456789101",
            new FullName(
                "Alex",
                "Bodui",
                "Fring"),
            DateTime.UtcNow
        );

        public static Founder FounderB = new Founder
        (
            (INN)"123456789102",
            new FullName(
                "Ban",
                "Broud",
                "Steli"),
            DateTime.UtcNow
        );

        public static IndividualEntrepreneur IndividualEntrepreneurA = new IndividualEntrepreneur
        (
            (INN)"225252525222",
            "IE Streem",
            DateTime.UtcNow,
            FounderA.INN
        );
        public static IndividualEntrepreneur IndividualEntrepreneurB = new IndividualEntrepreneur
        (
            (INN)"225252525223",
            "IE ITPEDIA",
            DateTime.UtcNow,
            FounderB.INN
        );

        public static LegalEntity LegalEntityA = new LegalEntity
        (
            (INN)"425252525222",
            "OOO Doom",
            DateTime.UtcNow,
            new List<Founder> { FounderA }
        );

        public static LegalEntity LegalEntityB = new LegalEntity
        (
            (INN)"525252525222",
            "OOO Latron",
            DateTime.UtcNow,
            new List<Founder> { FounderA, FounderB }
        );

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
                (
                    (INN)"123456789101",
                    new FullName(
                        "Alex",
                        "Bodui",
                        "Fring"),
                    DateTime.UtcNow
                ),
                new Founder
                (
                    (INN)"123456789102",
                    new FullName(
                        "Ban",
                        "Broud",
                        "Steli"),
                    DateTime.UtcNow
                )
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
