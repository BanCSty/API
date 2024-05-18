using System.Collections.Generic;

namespace API.Application.Founders.Queries.GetFounderList
{
    public class FounderListVm
    {
        public IList<FounderLookUpDto> Founders { get; set; }

        public FounderListVm(List<FounderLookUpDto> founders)
        {
            Founders = founders;
        }

        public FounderListVm()
        {
            Founders = new List<FounderLookUpDto>();
        }
    }
}
