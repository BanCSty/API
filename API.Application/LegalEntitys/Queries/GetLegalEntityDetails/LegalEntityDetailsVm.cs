using API.Application.Common.Mappings;
using API.Application.DTO;
using API.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityDetails
{
    //View model
    public class LegalEntityDetailsVm : IMapWith<LegalEntity>
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public List<FounderDto> Founders { get; set; }

        //Мапим LegalEntity к LegalEntityDetailsVm
        public void Mapping(Profile profile)
        {
            profile.CreateMap<LegalEntity, LegalEntityDetailsVm>()
                .ForMember(LEvm => LEvm.Id,
                    opt => opt.MapFrom(LE => LE.Id))
                .ForMember(LEvm => LEvm.INN,
                    opt => opt.MapFrom(LE => LE.INN))
                .ForMember(LEvm => LEvm.Name,
                    opt => opt.MapFrom(LE => LE.Name))
                .ForMember(LEvm => LEvm.DateCreate,
                    opt => opt.MapFrom(LE => LE.DateCreate))
                .ForMember(LEvm => LEvm.DateUpdate,
                    opt => opt.MapFrom(LE => LE.DateUpdate))
                .ForMember(LEDto => LEDto.Founders,
                    opt => opt.MapFrom(LE => LE.Founders != null ?
                    LE.Founders.Select(f => new FounderDto
                    {
                        Id = f.Id,
                        FirstName = f.FirstName,
                        LastName = f.LastName,
                        MiddleName = f.MiddleName,
                        INN = f.INN
                    }) : null));

        }
    }
}
