using System;
using System.Collections.Generic;

namespace API.Domain
{
    //ИП
    public class IndividualEntrepreneur
    {

        public Guid Id { get; set; }
        public long INN { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }

        public Guid FounderId { get; set; }

        // Учредитель ИП
        public virtual Founder Founder { get; set; }
    }
}
