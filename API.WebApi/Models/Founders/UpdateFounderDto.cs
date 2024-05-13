using API.Application.Common.Mappings;
using API.Application.Founders.Command.UpdateFounder;
using API.WebApi.Attributes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace API.WebApi.Models.Founders
{
    public class UpdateFounderDto : IMapWith<UpdateFounderCommand>
    {
        [Required]
        public Guid Id { get; set; }

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

        public List<Guid>? LegalEntityIds { get; set; }

        public Guid? IndividualEntrepreneurId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateFounderDto, UpdateFounderCommand>()
                .ForMember(founderCommand => founderCommand.Id,
                opt => opt.MapFrom(founderDto => founderDto.Id))
                .ForMember(founderCommand => founderCommand.INN,
                opt => opt.MapFrom(founderDto => founderDto.INN))
                .ForMember(founderCommand => founderCommand.FirstName,
                opt => opt.MapFrom(founderDto => founderDto.FirstName))
                .ForMember(founderCommand => founderCommand.LastName,
                opt => opt.MapFrom(founderDto => founderDto.LastName))
                .ForMember(founderCommand => founderCommand.MiddleName,
                opt => opt.MapFrom(founderDto => founderDto.MiddleName))
                .ForMember(founderCommand => founderCommand.LegalEntityIds,
                opt => opt.MapFrom(founderDto => founderDto.LegalEntityIds))
                .ForMember(founderCommand => founderCommand.IndividualEntrepreneurId,
                opt => opt.MapFrom(founderDto => founderDto.IndividualEntrepreneurId));
        }
    }
}
