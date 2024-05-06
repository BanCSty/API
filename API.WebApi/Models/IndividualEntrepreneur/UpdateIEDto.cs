using API.Application.Common.Mappings;
using API.Application.IndividualEntrepreneurs.Command.UpdateIE;
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
    //Update ИП|IndividualEntrepreneur
    public class UpdateIEDto : IMapWith<UpdateIECommand>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [INN12Digits]
        public long INN { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public Guid FounderId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateIEDto, UpdateIECommand>()
                .ForMember(IECommand => IECommand.Id,
                opt => opt.MapFrom(IEDto => IEDto.Id))
                .ForMember(IECommand => IECommand.INN,
                opt => opt.MapFrom(IEDto => IEDto.INN))
                .ForMember(IECommand => IECommand.Name,
                opt => opt.MapFrom(IEDto => IEDto.Name))
                .ForMember(dest => dest.FounderId, 
                opt => opt.MapFrom(src => src.FounderId != default ? src.FounderId : Guid.Empty));
        }
    }
}
