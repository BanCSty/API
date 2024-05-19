using API.Application.ViewModel;
using API.Domain;
using System.Collections.Generic;
using System.Linq;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityList
{
    public class LegalEntityLookUpDto
    {
        public LegalEntityLookUpDto(LegalEntity legalEntity)
        {
            INN = legalEntity.INN;
            Name = legalEntity.Name;

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
        public string Name { get; set; }
        public string INN { get; set; }
        public List<FounderVm> Founders { get; set; }
    }
}
