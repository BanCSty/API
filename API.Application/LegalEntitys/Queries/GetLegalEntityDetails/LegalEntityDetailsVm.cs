using API.Domain;
using AutoMapper;
using System;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityDetails
{
    //View model
    //Отобразим информацию без Id учредителей
    public class LegalEntityDetailsVm
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        //public List<Guid> FounderId { get; set; }

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
                    opt => opt.MapFrom(LE => LE.DateUpdate));
        }
    }
}
