using AutoMapper;
using HealthCare_Data.Identity;
using HealthCare_Models.DTOs;
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
            CreateMap<treatment, treatmentDto>()
                           .ForMember(dest => dest.TherapistName, opt => opt.MapFrom(src => src.Therapist.Name)) 
                           .ReverseMap();
            CreateMap<Assessment, AssessmentDto>()
                       .ForMember(dest => dest.SchedulerId, opt => opt.MapFrom(src => src.schedulerId))
                       .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.schedulerDate.treatment.ApplicationUser.UserName))
                       .ForMember(dest => dest.TherapistName, opt => opt.MapFrom(src => src.schedulerDate.treatment.Therapist.Name))
                       .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.schedulerDate.treatment.ApplicationUser.UserName))
                       .ForMember(dest => dest.Treatment, opt => opt.MapFrom(src => src.schedulerDate.treatment.Title))
                       .ForMember(dest => dest.ScheduleDate, opt => opt.MapFrom(src => src.schedulerDate.dateFrom))
                       .ReverseMap()
                       .ForMember(dest => dest.schedulerDate, opt => opt.Ignore());
        }
    }
}



