using AutoMapper;
using HealthCare_Data.Identity;
using HealthCare_Models.DTOs;
using HealthCareData.Identity;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repositorys.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IMapper _mapper;

        public TreatmentController(ITreatmentRepository treatmentRepository, IMapper mapper)
        {
            _treatmentRepository = treatmentRepository;
            _mapper = mapper;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var treatments = await _treatmentRepository.GetAllAsync();

            // 👇 THIS IS THE LINE YOU ASKED ABOUT
            var result = _mapper.Map<IEnumerable<treatmentDto>>(treatments);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var treatment = await _treatmentRepository.GetByIdAsync(id);
            if (treatment == null) return NotFound();
            var result = _mapper.Map<treatmentDto>(treatment);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] treatmentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var treatment = new treatment
            {
                Title = dto.Title,
                DateCreated = dto.DateCreated,
                TherapistId = dto.TherapistId,              
                ApplicationUserId = dto.ApplicationUserId  
            };

            var created = await _treatmentRepository.CreateAsync(treatment);
            return Ok(_mapper.Map<treatmentDto>(created));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] treatmentDto dto)
        {
            var treatment = _mapper.Map<treatment>(dto);
            var updated = await _treatmentRepository.UpdateAsync(treatment);
            return Ok(_mapper.Map<treatmentDto>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _treatmentRepository.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
