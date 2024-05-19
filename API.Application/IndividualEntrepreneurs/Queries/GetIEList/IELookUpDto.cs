using API.Application.ViewModel;
using API.Domain;
using System;


namespace API.Application.IndividualEntrepreneurs.Queries.GetIEList
{
    public class IELookUpDto
    {
        public IELookUpDto(IndividualEntrepreneur individual)
        {
            Id = individual.Id;
            INN = individual.INN;
            Name = individual.Name;

            Founder = individual.Founder != null
                ? new FounderVm
                {
                    Id = individual.Founder.Id,
                    FirstName = individual.Founder.FirstName,
                    LastName = individual.Founder.LastName,
                    MiddleName = individual.Founder.MiddleName,
                    INN = individual.Founder.INN
                }
                : null;
        }

        public Guid Id { get; set; }
        public string INN { get; set; }
        public string Name { get; set; }

        public FounderVm Founder { get; set; }
    }
}
