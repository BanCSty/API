using MediatR;
using System;

namespace API.Application.Founders.Command.CreateFounder
{
    //Содержит информацию, о том, что необходимо для создания учредителя
    public class CreateFounderCommand : IRequest<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}
