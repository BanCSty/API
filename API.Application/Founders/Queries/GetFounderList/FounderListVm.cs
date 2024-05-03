using API.Domain;
using System.Collections.Generic;

namespace API.Application.Founders.Queries.GetFounderList
{
    public class FounderListVm
    {
        public IList<Founder> Founders { get; set; }
    }
}
