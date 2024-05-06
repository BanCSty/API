using API.Application.Common.Mappings;
using API.Application.DTO;
using API.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityList
{
    //Будем выводить Id, Name, INN, FounderDto
    public class LegalEntityLookUpDto : IMapWith<LegalEntity>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long INN { get; set; }
        public List<FounderDto> Founders { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LegalEntity, LegalEntityLookUpDto>()
                .ForMember(LEDto => LEDto.Id,
                    opt => opt.MapFrom(LE => LE.Id))
                .ForMember(LEDto => LEDto.Name,
                    opt => opt.MapFrom(LE => LE.Name))
                .ForMember(LEDto => LEDto.INN,
                    opt => opt.MapFrom(LE => LE.INN))
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
