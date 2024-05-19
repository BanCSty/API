using MediatR;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEDetails
{
    public class GetIEDetailsQuery : IRequest<IEDetailsVm>
    {
        public string INN { get; set; }
    }
}
