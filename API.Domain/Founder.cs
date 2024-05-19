using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Domain
{
    //Учредитель
    public class Founder
    {
        public Founder()
        {
            LegalEntities = new List<LegalEntity>();
        }

        public Guid Id { get; set; }
        public string INN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }

        //Юридическое лицо, к которому привязан учредитель
        public virtual ICollection<LegalEntity>? LegalEntities { get; set; }

        // Индивидуальный предприниматель, к которому привязан учредитель
        public virtual IndividualEntrepreneur? IndividualEntrepreneur { get; set; }
    }
}
