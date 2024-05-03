using API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Founders.Queries.GetFounderList
{
    public class FounderListVm
    {
        public IList<Founder> Founders { get; set; }
    }
}
