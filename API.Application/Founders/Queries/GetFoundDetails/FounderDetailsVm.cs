using API.Application.ViewModel;
using API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class FounderDetailsVm
    {
        public FounderDetailsVm(Founder founder)
        {
            Id = founder.Id;
            INN = founder.INN;
            FirstName = founder.FirstName;
            LastName = founder.LastName;
            MiddleName = founder.MiddleName;
            DateCreate = founder.DateCreate;
            DateUpdate = founder.DateUpdate;
            

            individualEntrepreneurs = founder.IndividualEntrepreneur != null
                ? new IndividualEntrepreneursVm
                {
                    Id = founder.IndividualEntrepreneur.Id,
                    Name = founder.IndividualEntrepreneur.Name,
                    INN = founder.IndividualEntrepreneur.INN
                }
                : null;

            LegalEntities = founder.LegalEntities != null && founder.LegalEntities.Any()
                ? founder.LegalEntities.Select(LE => new LegalEntityVm
                {
                    Id = LE.Id,
                    Name = LE.Name,
                    INN = LE.INN
                }).ToList()
                : null;
        }

        public Guid Id { get; set; }
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
