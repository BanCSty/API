using API.Application.ViewModel;
using API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Application.Founders.Queries.GetFounderList
{
    public class FounderLookUpDto
    {
        public FounderLookUpDto(Founder founder)
        {
            INN = founder.INN;
            FirstName = founder.FullName.FirstName;
            LastName = founder.FullName.LastName;
            MiddleName = founder.FullName.MiddleName;
            DateCreate = founder.DateCreate;
            DateUpdate = founder.DateUpdate;


            individualEntrepreneurs = founder.IndividualEntrepreneur != null
                ? new IndividualEntrepreneursVm
                {
                    Name = founder.IndividualEntrepreneur.Name,
                    INN = founder.IndividualEntrepreneur.INN
                }
                : null;

            LegalEntities = founder.LegalEntities != null && founder.LegalEntities.Any()
                ? founder.LegalEntities.Select(LE => new LegalEntityVm
                {
                    Name = LE.Name,
                    INN = LE.INN
                }).ToList()
                : null;
        }

        public string INN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }

        public List<LegalEntityVm>? LegalEntities { get; set; }

        public IndividualEntrepreneursVm? individualEntrepreneurs { get; set; }        
    }
}
