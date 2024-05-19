using MediatR;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class GetFounderDetailsQuery : IRequest<FounderDetailsVm>
    {
        public string INN { get; set; }
    }
}
