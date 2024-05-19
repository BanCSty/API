using API.Application.ViewModel;
using API.Domain;
using System;


namespace API.Application.IndividualEntrepreneurs.Queries.GetIEDetails
{
    public class IEDetailsVm
    {
        public IEDetailsVm(IndividualEntrepreneur individual)
        {
            INN = individual.INN;
            Name = individual.Name;
            DateCreate = individual.DateCreate;
            DateUpdate = individual.DateUpdate;

            Founder = individual.Founder != null
                ? new FounderVm
                {
                    FirstName = individual.Founder.FullName.FirstName,
                    LastName = individual.Founder.FullName.LastName,
                    MiddleName = individual.Founder.FullName.MiddleName,
                    INN = individual.Founder.INN
                }
                : null;
        }
        public string INN { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }

        public FounderVm Founder { get; set; }     
    }
}
