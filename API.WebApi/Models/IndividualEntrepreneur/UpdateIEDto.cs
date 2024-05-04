using API.Application.IndividualEntrepreneurs.Command.UpdateIE;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.WebApi.Models.IndividualEntrepreneur
{
    public class UpdateIEDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public long INN { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateIEDto, UpdateIECommand>()
                .ForMember(IECommand => IECommand.Id,
                opt => opt.MapFrom(IEDto => IEDto.Id))
                .ForMember(IECommand => IECommand.INN,
                opt => opt.MapFrom(IEDto => IEDto.INN))
                .ForMember(IECommand => IECommand.Name,
                opt => opt.MapFrom(IEDto => IEDto.Name));
        }
    }
}
