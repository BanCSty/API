using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain
{
    //ИП
    public class IndividualEntrepreneur
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }

        public virtual Founder Founder { get; set; }
    }
}
