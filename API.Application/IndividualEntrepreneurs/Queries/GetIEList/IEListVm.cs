using API.Application.Common.Mappings;
using API.Application.DTO;
using API.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEList
{
    public class IEListVm : IMapWith<List<IEListVm>>
    {
        public IList<IELookUpDto> IndividualEntrepreneurs { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<List<IELookUpDto>, IEListVm>()
                .ForMember(dest => dest.IndividualEntrepreneurs, opt => opt.MapFrom(src => src));
        }
    }
}
