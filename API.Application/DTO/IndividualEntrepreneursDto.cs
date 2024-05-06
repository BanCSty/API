using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.DTO
{
    public class IndividualEntrepreneursDto
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string Name { get; set; }
    }
}
