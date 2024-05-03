using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain
{
    //Промежуточная таблица для связи многие ко многим таблиц Founder и LegalEntity
    public class LegalEntityFounder
    {
        public Guid LegalEntityId { get; set; }
        public LegalEntity LegalEntity { get; set; }

        public Guid FounderId { get; set; }
        public Founder Founder { get; set; }
    }
}
