using API.Application.LegalEntitys.Command.UpdateLegalEntity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.WebApi.Models.LegalEntity
{
    public class UpdateLegalEntityDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public long INN { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public List<Guid> FounderId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateLegalEntityDto, UpdateLegalEntityCommand>()
                .ForMember(LECommand => LECommand.Id,
                opt => opt.MapFrom(LEDto => LEDto.Id))
                .ForMember(LECommand => LECommand.INN,
                opt => opt.MapFrom(LEDto => LEDto.INN))
                .ForMember(LECommand => LECommand.Name,
                opt => opt.MapFrom(LEDto => LEDto.Name))
                .ForMember(LECommand => LECommand.FounderId,
                opt => opt.MapFrom(LEDto => LEDto.FounderId));
        }
    }
}
