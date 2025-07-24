using AutoMapper;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseaseController : ControllerBase
    {
        private readonly IDiseaseRepository _diseaseRepo;
        private readonly IMapper _mapper;

        public DiseaseController(IDiseaseRepository diseaseRepo, IMapper mapper)
        {
            _diseaseRepo = diseaseRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetAll()
        {
            var diseases = await _diseaseRepo.GetAllAsync();
            return Ok(diseases);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiseaseDto>> Get(int id)
        {
            var disease = await _diseaseRepo.GetByIdAsync(id);
            if (disease == null)
                return NotFound();

            return Ok(disease);
        }

        [HttpPost("Upsert")]
        public async Task<ActionResult<DiseaseDto>> Upsert([FromBody] DiseaseDto dto)
        {
            try
            {
                if (dto.TherapistId == 0)
                    return BadRequest("Therapist must be selected.");

                if (dto.CaseId > 0)
                {
                    var existing = await _diseaseRepo.GetByIdAsync(dto.CaseId);
                    if (existing == null)
                        return NotFound($"Case with ID {dto.CaseId} not found.");
                    await _diseaseRepo.UpdateAsync(dto);
                }
                else
                {
                    await _diseaseRepo.AddAsync(dto);
                }

                await _diseaseRepo.SaveAsync();
                var result = await _diseaseRepo.GetByIdAsync(dto.CaseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _diseaseRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _diseaseRepo.DeleteAsync(id);
            await _diseaseRepo.SaveAsync();
            return NoContent();
        }
    }
}
