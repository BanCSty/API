using API.Application.Common.Mappings;
using API.Application.Founders.Command.CreateFounder;
using API.WebApi.Attributes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.WebApi.Models.Founders
{
    public class CreateFounderDto : IMapWith<CreateFounderCommand>
    {
        [Required]
        [INN12Digits]
        public long INN { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string MiddleName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateFounderDto, CreateFounderCommand>()
                .ForMember(founderCommand => founderCommand.INN,
                opt => opt.MapFrom(founderDto => founderDto.INN))
                .ForMember(founderCommand => founderCommand.FirstName,
                opt => opt.MapFrom(founderDto => founderDto.FirstName))
                .ForMember(founderCommand => founderCommand.LastName,
                opt => opt.MapFrom(founderDto => founderDto.LastName))
                .ForMember(founderCommand => founderCommand.MiddleName,
                opt => opt.MapFrom(founderDto => founderDto.MiddleName));
        }
    }
}
