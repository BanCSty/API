using API.Application.ViewModel;
using API.Domain;


namespace API.Application.IndividualEntrepreneurs.Queries.GetIEList
{
    public class IELookUpDto
    {
        public IELookUpDto(IndividualEntrepreneur individual)
        {
            INN = individual.INN;
            Name = individual.Name;

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

        public FounderVm Founder { get; set; }
    }
}
