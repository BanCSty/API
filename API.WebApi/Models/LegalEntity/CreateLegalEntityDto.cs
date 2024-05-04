using API.Application.LegalEntitys.Command.CreateLegalEntity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.WebApi.Models.LegalEntity
{
    public class CreateLegalEntityDto
    {
        [Required]
        public long INN { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public List<Guid> FounderId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateLegalEntityDto, CreateLegalEntityCommand>()
                .ForMember(LECommand => LECommand.INN,
                opt => opt.MapFrom(LEDto => LEDto.INN))
                .ForMember(LECommand => LECommand.Name,
                opt => opt.MapFrom(LEDto => LEDto.Name))
                .ForMember(LECommand => LECommand.FounderId,
                opt => opt.MapFrom(LEDto => LEDto.FounderId));
        }
    }
}
