using AutoMapper;
using HealthCareData.Identity;
using HealthCareModels.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HealthCareModels.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, PatientDto>().ReverseMap();
            CreateMap<CaseDto, Case>()
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.PatientId))
                .ReverseMap();
            CreateMap<Disease, DiseaseDto>().ReverseMap();
        }
    }
}



