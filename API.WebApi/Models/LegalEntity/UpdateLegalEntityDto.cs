using API.Application.LegalEntitys.Command.UpdateLegalEntity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Domain;
using API.Application.Common.Mappings;
using API.WebApi.Attributes;

namespace API.WebApi.Models.LegalEntity
{
    public class UpdateLegalEntityDto : IMapWith<UpdateLegalEntityCommand>
    {
        [Required]
        public Guid Id { get; set; }

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
            profile.CreateMap<UpdateLegalEntityDto, UpdateLegalEntityCommand>()
                .ForMember(LECommand => LECommand.Id,
                opt => opt.MapFrom(LEDto => LEDto.Id))
                .ForMember(LECommand => LECommand.INN,
                opt => opt.MapFrom(LEDto => LEDto.INN))
                .ForMember(LECommand => LECommand.Name,
                opt => opt.MapFrom(LEDto => LEDto.Name))
                .ForMember(LECommand => LECommand.FounderIds,
                opt => opt.MapFrom(LEDto =>  LEDto.FounderIds));
        }
    }
}
