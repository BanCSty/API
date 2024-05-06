using API.Application.Common.Mappings;
using API.Application.LegalEntitys.Command.CreateLegalEntity;
using API.Domain;
using API.WebApi.Attributes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.WebApi.Models.LegalEntity
{
    public class CreateLegalEntityDto : IMapWith<CreateLegalEntityCommand>
    {
        [Required]
        [INN12Digits]
        public long INN { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public List<Guid> FounderIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateLegalEntityDto, CreateLegalEntityCommand>()
                .ForMember(LECommand => LECommand.INN,
                opt => opt.MapFrom(LEDto => LEDto.INN))
                .ForMember(LECommand => LECommand.Name,
                opt => opt.MapFrom(LEDto => LEDto.Name))
                .ForMember(LECommand => LECommand.FounderIds,
                opt => opt.MapFrom(LEDto => LEDto.FounderIds));
        }
    }
}
