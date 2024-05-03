using API.Domain;
using AutoMapper;
using System;


namespace API.Application.LegalEntitys.Queries.GetLegalEntityList
{
    //Будем выводить Name, INN, DateCreate
    public class LegalEntityLookUpDto
    {
        public string Name { get; set; }
        public long INN { get; set; }
        public DateTime DateCreate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LegalEntity, LegalEntityLookUpDto>()
                .ForMember(LEDto => LEDto.Name,
                    opt => opt.MapFrom(LE => LE.Name))
                .ForMember(LEDto => LEDto.INN,
                    opt => opt.MapFrom(LE => LE.INN))
                .ForMember(LEDto => LEDto.DateCreate,
                    opt => opt.MapFrom(LE => LE.DateCreate));
        }
    }
}
