using AutoMapper;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare_API.Controllers
{
    [ApiController]
    [Route("api/Therapist")]
    public class TherapistController : Controller
    {
        private readonly ITherapistRepository _therapistRepository;
        private readonly IMapper _mapper;

        public TherapistController(ITherapistRepository therapistRepository, IMapper mapper)
        {
            _therapistRepository = therapistRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TherapistDto>> GetAllTherapists()
        {
            return await _therapistRepository.GetAllTherapistsAsync();
        }

        [HttpGet("{id}")]
        public async Task<TherapistDto> GetTherapist(int id)
        {
            return await _therapistRepository.GetTherapistByIdAsync(id);
        }

        [HttpPost("Upsert")]
        public async Task<TherapistDto> UpsertTherapist([FromBody] TherapistDto therapistDto)
        {
            if (therapistDto.TherapistId == 0)
            {
                return await _therapistRepository.CreateTherapistAsync(therapistDto);
            }
            else
            {
                return await _therapistRepository.UpdateTherapistAsync(therapistDto);
            }
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteTherapist(int id)
        {
            return await _therapistRepository.DeleteTherapistAsync(id);
        }
    }
}
