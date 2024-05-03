using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityList
{
    //ViewModel
    public class LegalEntityListVm
    {
        public IList<LegalEntityLookUpDto> LegalEntitys { get; set; }
    }
}
