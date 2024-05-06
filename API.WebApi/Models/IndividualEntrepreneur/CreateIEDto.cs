using API.Application.Common.Mappings;
using API.Application.IndividualEntrepreneurs.Command.CreateIE;
using API.WebApi.Attributes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.WebApi.Models.IndividualEntrepreneur
{
    //Create ИП|IndividualEntrepreneur
    public class CreateIEDto : IMapWith<CreateIECommand>
    {
        [Required]
        [INN12Digits]
        public long INN { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public Guid FounderId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateIEDto, CreateIECommand>()
                .ForMember(IECommand => IECommand.FounderId,
                opt => opt.MapFrom(IEDto => IEDto.FounderId))
                .ForMember(IECommand => IECommand.INN,
                opt => opt.MapFrom(IEDto => IEDto.INN))
                .ForMember(IECommand => IECommand.Name,
                opt => opt.MapFrom(IEDto => IEDto.Name));
        }
    }
}
