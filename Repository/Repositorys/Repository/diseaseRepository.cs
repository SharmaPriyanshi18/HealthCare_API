using HealthCareData.Identity;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository
{
    public class DiseaseRepository : IDiseaseRepository
    {
        private readonly ApplicationDbContext _context;

        public DiseaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DiseaseDto dto)
        {
            var entity = new Case
            {
                Title = dto.Title,
                DateCreated = dto.DateCreated,
                ApplicationUserId = dto.ApplicationUserId,
                TherapistId = dto.TherapistId,
            };

            await _context.Cases.AddAsync(entity);
            await _context.SaveChangesAsync();

            dto.CaseId = entity.CaseId;

            // Optionally fill therapist name/address immediately in response DTO
            var therapist = await _context.Therapists
                .FirstOrDefaultAsync(t => t.TherapistId == dto.TherapistId);

            dto.TherapistName = therapist?.Name;
            dto.Address = therapist?.Address;
        }

        public async Task DeleteAsync(int caseId)
        {
            var entity = await _context.Cases.FindAsync(caseId);
            if (entity != null)
            {
                _context.Cases.Remove(entity);
            }
        }

        public async Task<IEnumerable<DiseaseDto>> GetAllAsync()
        {
            return await _context.Cases
                .Include(c => c.Therapist)
                .Select(c => new DiseaseDto
                {
                    CaseId = c.CaseId,
                    Title = c.Title,
                    DateCreated = c.DateCreated,
                    ApplicationUserId = c.ApplicationUserId,
                    TherapistId = c.TherapistId,
                    TherapistName = c.Therapist != null ? c.Therapist.Name : "",
                    Address = c.Therapist != null ? c.Therapist.Address : ""
                })
                .ToListAsync();
        }

        public async Task<DiseaseDto> GetByIdAsync(int caseId)
        {
            var c = await _context.Cases
                .Include(x => x.Therapist)
                .FirstOrDefaultAsync(x => x.CaseId == caseId);

            if (c == null) return null;

            return new DiseaseDto
            {
                CaseId = c.CaseId,
                Title = c.Title,
                DateCreated = c.DateCreated,
                ApplicationUserId = c.ApplicationUserId,
                TherapistId = c.TherapistId,
                TherapistName = c.Therapist != null ? c.Therapist.Name : "",
                Address = c.Therapist != null ? c.Therapist.Address : ""
            };
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DiseaseDto dto)
        {
            var entity = await _context.Cases.FindAsync(dto.CaseId);
            if (entity == null) return;

            entity.Title = dto.Title;
            entity.DateCreated = dto.DateCreated;
            entity.ApplicationUserId = dto.ApplicationUserId;
            entity.TherapistId = dto.TherapistId;

            _context.Cases.Update(entity);
        }
    }
}
