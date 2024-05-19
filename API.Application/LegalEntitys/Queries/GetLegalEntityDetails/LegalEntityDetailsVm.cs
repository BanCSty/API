using API.Application.ViewModel;
using API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityDetails
{
    //View model
    public class LegalEntityDetailsVm
    {
        public LegalEntityDetailsVm(LegalEntity legalEntity)
        {
            INN = legalEntity.INN;
            Name = legalEntity.Name;
            DateCreate = legalEntity.DateCreate;
            DateUpdate = legalEntity.DateUpdate;

            Founders = legalEntity.Founders != null && legalEntity.Founders.Any()
                ? legalEntity.Founders.Select(f => new FounderVm
                {
                    FirstName = f.FullName.FirstName,
                    LastName = f.FullName.LastName,
                    MiddleName = f.FullName.MiddleName,
                    INN = f.INN

                }).ToList()
                : null;
        }

        public string INN { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public List<FounderVm> Founders { get; set; }     
    }
}
