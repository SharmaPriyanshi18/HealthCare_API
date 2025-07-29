using AutoMapper;
using HealthCare_Models.DTOs;
using HealthCareData;
using HealthCareData.Identity;
using HealthCareRepositorys.Repositorys.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentRepository _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public AssessmentController(
            IAssessmentRepository repository,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var assessments = await _repository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<AssessmentDto>>(assessments);
            return Ok(result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var assessment = await _repository.GetByIdAsync(id);
            if (assessment == null)
                return NotFound();

            var result = _mapper.Map<AssessmentDto>(assessment);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AssessmentDto dto)
        {
            var scheduler = await _context.schedulers.FindAsync(dto.SchedulerId);
            if (scheduler == null)
                return BadRequest($"Scheduler with ID {dto.SchedulerId} does not exist.");

            var assessment = _mapper.Map<Assessment>(dto);

            assessment.schedulerDate = scheduler;
            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();

            return Ok("Assessment created successfully.");
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AssessmentDto dto)
        {
            if (id != dto.AssessmentId)
                return BadRequest("ID mismatch.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
