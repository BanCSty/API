
using API.Application.Common.Mappings;
using AutoMapper;
using System.Collections.Generic;


namespace API.Application.LegalEntitys.Queries.GetLegalEntityList
{
    //ViewModel
    public class LegalEntityListVm : IMapWith<List<LegalEntityLookUpDto>>
    {
        public IList<LegalEntityLookUpDto> LegalEntitys { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<List<LegalEntityLookUpDto>, LegalEntityListVm>()
                .ForMember(dest => dest.LegalEntitys, opt => opt.MapFrom(src => src));
        }
    }
}
