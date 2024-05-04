using API.Application.Founders.Command.UpdateFounder;
using AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;


namespace API.WebApi.Models.Founder
{
    public class UpdateFounderDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
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
            profile.CreateMap<UpdateFounderDto, UpdateFounderCommand>()
                .ForMember(founderCommand => founderCommand.FounderId,
                opt => opt.MapFrom(founderDto => founderDto.Id))
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
