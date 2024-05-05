using System;
using System.Collections.Generic;

namespace API.Domain
{
    //ЮЛ
    public class LegalEntity
    {
        public LegalEntity()
        {
            Founders = new List<Founder>();
        }

        public Guid Id { get; set; }
        public long INN { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }

        public List<Founder> Founders { get; set; }
    }
}
