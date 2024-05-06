using API.Application.Common.Mappings;
using API.Domain;
using AutoMapper;
using System.Collections.Generic;

namespace API.Application.Founders.Queries.GetFounderList
{
    public class FounderListVm : IMapWith<List<FounderLookUpDto>>
    {
        public IList<FounderLookUpDto> Founders { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<List<FounderLookUpDto>, FounderListVm>()
                .ForMember(dest => dest.Founders, opt => opt.MapFrom(src => src));
        }
    }
}
