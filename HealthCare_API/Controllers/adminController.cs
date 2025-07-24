using AutoMapper;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_API.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IPatientProfileRepository _patientRepo;
        private readonly IMapper _mapper;


        public AdminController(IPatientProfileRepository patientRepo,IMapper mapper)
        {
            _patientRepo = patientRepo;
            _mapper = mapper;
        }

        [HttpGet("patients")]
        public async Task<List<PatientDto>> GetAllPatients()
        {
            var patients = await _patientRepo.GetAllAsync();
            return patients.ToList();
        }

        [HttpGet("patients/{id}")]
        public async Task<PatientDto> GetPatientById(string id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            return patient!;
        }

        [HttpPost("patients")]
        public async Task<PatientDto> UpsertPatient([FromBody] PatientDto patient)
        {
            if (patient.Id==null)
            {
                await _patientRepo.AddAsync(patient);
            }
            else
            {
                await _patientRepo.UpdateAsync(patient);
            }

            await _patientRepo.SaveAsync();
            return patient;
        }

        [HttpDelete("patients/{id}")]
        public async Task<PatientDto> DeletePatient(string id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient != null)
            {
                await _patientRepo.DeleteAsync(id);
                await _patientRepo.SaveAsync();
            }
            return patient!;
        }
    }
}
