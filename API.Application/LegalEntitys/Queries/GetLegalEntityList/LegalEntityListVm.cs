using System.Collections.Generic;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityList
{
    //ViewModel
    public class LegalEntityListVm
    {
        public IList<LegalEntityLookUpDto> LegalEntitys { get; set; }

        public LegalEntityListVm(List<LegalEntityLookUpDto> legalEntityLookUps)
        {
            LegalEntitys = legalEntityLookUps;
        }

        public LegalEntityListVm()
        {
            LegalEntitys = new List<LegalEntityLookUpDto>();
        }
    }
}
