﻿using System;
using System.Collections.Generic;

namespace API.Domain
{
    //Учредитель
    public class Founder
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }

        public virtual ICollection<LegalEntity> LegalEntities { get; set; }
    }
}
