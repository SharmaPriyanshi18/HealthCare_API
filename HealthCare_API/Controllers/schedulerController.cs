using HealthCare_Data.Identity;
using HealthCareData.Identity;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_API.Controllers
{
    [Route("api/scheduler")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly IShedularRepository _schedulerRepo;
        private readonly ApplicationDbContext _context;
        private readonly ISchedulerQueueService _queueService;

        public SchedulerController(
            IShedularRepository schedulerRepo,
            ApplicationDbContext context,
            ISchedulerQueueService queueService)
        {
            _schedulerRepo = schedulerRepo;
            _context = context;
            _queueService = queueService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var schedulerDtos = await _context.schedulers
                .Include(s => s.treatment)
                    .ThenInclude(d => d.ApplicationUser)
                .Include(s => s.SchedulerTherapists)
                    .ThenInclude(st => st.Therapist)
                .Select(s => new schedularDto
                {
                    SchedulerId = s.schedulerId,
                    DateFrom = s.dateFrom,
                    DateTo = s.dateTo,
                    treatmentId = s.treatmentId,
                    Title = s.treatment.Title,
                    ApplicationUserId = s.treatment.ApplicationUserId,
                    UserName = s.treatment.ApplicationUser.UserName,
                    address = s.treatment.ApplicationUser.Address,
                    phonenumber = s.treatment.ApplicationUser.PhoneNumber,
                    email = s.treatment.ApplicationUser.Email,
                    TherapistIds = string.Join(",", s.SchedulerTherapists.Select(st => st.TherapistId)),
                    TherapistNames = string.Join(", ", s.SchedulerTherapists.Select(st => st.Therapist.Name))
                })
                .ToListAsync();

            return Ok(schedulerDtos);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] schedularDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TherapistIds))
                return BadRequest("At least one therapist must be selected.");

            var therapistIdList = dto.TherapistIds.Split(',').Select(int.Parse).ToList();

            var treatment = await _context.treatments
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.treatmentId == dto.treatmentId);

            if (treatment == null)
                return BadRequest("Invalid CaseId.");

            treatment.TherapistId = therapistIdList.First(); 

            var scheduler = new schedulerDate
            {
                dateFrom = dto.DateFrom,
                dateTo = dto.DateTo,
                treatmentId = dto.treatmentId,
                treatment = treatment,
                SchedulerTherapists = therapistIdList.Select(tid => new SchedulerTherapist
                {
                    TherapistId = tid
                }).ToList()
            };

            await _schedulerRepo.CreateAsync(scheduler);
            await _schedulerRepo.SaveAsync();

            return Ok(new { message = "Scheduler created with selected therapists." });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] schedularDto dto)
        {
            if (id != dto.SchedulerId)
                return BadRequest("SchedulerId mismatch.");

            var scheduler = await _context.schedulers
                .Include(s => s.SchedulerTherapists)
                .FirstOrDefaultAsync(s => s.schedulerId == id);

            if (scheduler == null)
                return NotFound("Scheduler not found.");

            scheduler.dateFrom = dto.DateFrom;
            scheduler.dateTo = dto.DateTo;
            scheduler.treatmentId = dto.treatmentId;

            var therapistIdList = dto.TherapistIds.Split(',').Select(int.Parse).ToList();

            _context.schedulerTherapists.RemoveRange(scheduler.SchedulerTherapists);

            scheduler.SchedulerTherapists = therapistIdList.Select(tid => new SchedulerTherapist
            {
                TherapistId = tid,
                SchedulerId = scheduler.schedulerId
            }).ToList();

            await _schedulerRepo.UpdateAsync(scheduler);
            await _schedulerRepo.SaveAsync();

            return Ok(new { message = "Scheduler updated successfully." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _schedulerRepo.DeleteAsync(id);
            if (!success)
                return NotFound("Scheduler not found.");

            await _schedulerRepo.SaveAsync(); 
            return Ok(new { message = "Scheduler deleted successfully." });
        }
    }
}
