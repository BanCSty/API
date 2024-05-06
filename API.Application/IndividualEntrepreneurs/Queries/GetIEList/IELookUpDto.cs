using API.Application.Common.Mappings;
using API.Application.DTO;
using API.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEList
{
    public class IELookUpDto : IMapWith<IndividualEntrepreneur>
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string Name { get; set; }

        public FounderDto Founder { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IndividualEntrepreneur, IELookUpDto>()
                .ForMember(IEVm => IEVm.Id,
                    opt => opt.MapFrom(IE => IE.Id))
                .ForMember(IEVm => IEVm.INN,
                    opt => opt.MapFrom(IE => IE.INN))
                .ForMember(IEVm => IEVm.Name,
                    opt => opt.MapFrom(IE => IE.Name))
                .ForMember(IEVm => IEVm.Founder,
                    opt => opt.MapFrom(founder => founder.Founder != null ?
                        new FounderDto
                        {
                            Id = founder.Founder.Id,
                            INN = founder.Founder.INN,
                            FirstName = founder.Founder.FirstName,
                            LastName = founder.Founder.LastName
                        } : null));
        }
    }
}
