﻿using API.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Interfaces
{
    public interface IApiDbContext
    {
        DbSet<Founder> Founders { get; set; }
        DbSet<LegalEntity> LegalEntitys { get; set; }
        DbSet<IndividualEntrepreneur> IndividualEntrepreneurs { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
