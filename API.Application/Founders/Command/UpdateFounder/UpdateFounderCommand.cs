using API.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;

namespace API.Application.Founders.Command.UpdateFounder
{
    public class UpdateFounderCommand : IRequest
    {
        public string INN { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string? IndividualEntrepreneurINN { get; set; }
        public List<string>? LegalEntityINNs { get; set; } = new List<string>();
    }
}
