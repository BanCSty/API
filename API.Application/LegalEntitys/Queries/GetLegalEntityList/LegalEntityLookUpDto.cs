using API.Application.ViewModel;
using API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityList
{
    public class LegalEntityLookUpDto
    {
        public LegalEntityLookUpDto(LegalEntity legalEntity)
        {
            Id = legalEntity.Id;
            INN = legalEntity.INN;
            Name = legalEntity.Name;

            Founders = legalEntity.Founders != null && legalEntity.Founders.Any()
                ? legalEntity.Founders.Select(f => new FounderVm
                {
                    Id = f.Id,
                    FirstName = f.FirstName,
                    LastName = f.LastName,
                    MiddleName = f.MiddleName,
                    INN = f.INN

                }).ToList()
                : null;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string INN { get; set; }
        public List<FounderVm> Founders { get; set; }
    }
}
