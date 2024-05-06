using API.Application.Common.Mappings;
using API.Application.DTO;
using API.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Application.Founders.Queries.GetFounderList
{
    public class FounderLookUpDto : IMapWith<Founder>
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }

        public List<LegalEntityDto> LegalEntities { get; set; }

        public IndividualEntrepreneursDto individualEntrepreneurs { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Founder, FounderLookUpDto>()
                .ForMember(FounderVm => FounderVm.Id,
                    opt => opt.MapFrom(founder => founder.Id))
                .ForMember(FounderVm => FounderVm.INN,
                    opt => opt.MapFrom(founder => founder.INN))
                .ForMember(FounderVm => FounderVm.FirstName,
                    opt => opt.MapFrom(founder => founder.FirstName))
                .ForMember(FounderVm => FounderVm.LastName,
                    opt => opt.MapFrom(founder => founder.LastName))
                .ForMember(FounderVm => FounderVm.MiddleName,
                    opt => opt.MapFrom(founder => founder.MiddleName))
                .ForMember(FounderVm => FounderVm.DateCreate,
                    opt => opt.MapFrom(founder => founder.DateCreate))
                .ForMember(FounderVm => FounderVm.DateUpdate,
                    opt => opt.MapFrom(founder => founder.DateUpdate))
                .ForMember(FounderVm => FounderVm.LegalEntities,
                    opt => opt.MapFrom(founder => founder.LegalEntities != null ?
                        founder.LegalEntities.Select(le => new LegalEntityDto
                        {
                            Id = le.Id,
                            INN = le.INN,
                            Name = le.Name
                        }) : null))
                .ForMember(FounderVm => FounderVm.individualEntrepreneurs,
                    opt => opt.MapFrom(f => f.IndividualEntrepreneur != null
                        ? new IndividualEntrepreneursDto
                        {
                            Id = f.IndividualEntrepreneur.Id,
                            INN = f.IndividualEntrepreneur.INN,
                            Name = f.IndividualEntrepreneur.Name
                        }
                        : null));

        }
    }
}
