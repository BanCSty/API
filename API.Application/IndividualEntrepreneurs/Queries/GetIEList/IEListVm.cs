using System.Collections.Generic;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEList
{
    public class IEListVm
    {
        public IList<IELookUpDto> IndividualEntrepreneurs { get; set; }

        public IEListVm(List<IELookUpDto> iELookUp)
        {
            IndividualEntrepreneurs = iELookUp;
        }

        public IEListVm()
        {
            IndividualEntrepreneurs = new List<IELookUpDto>();
        }
    }
}
