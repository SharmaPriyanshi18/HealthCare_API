using HealthCare_Data.Identity;
using HealthCareData.Identity;
using HealthCareRepositorys.Repositorys.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repositorys.Repository
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AssessmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Assessment> CreateAsync(Assessment entity)
        {
            await _context.Assessments.AddAsync(entity);
            await SaveAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Assessments.FindAsync(id);
            if (entity == null)
                return false;

            _context.Assessments.Remove(entity);
            return await SaveAsync();
        }

        public async Task<IEnumerable<Assessment>> GetAllAsync()
        {
            return await _context.Assessments
                           .Include(a => a.schedulerDate)
                               .ThenInclude(s => s.treatment)
                                   .ThenInclude(t => t.ApplicationUser)
                           .Include(a => a.schedulerDate.SchedulerTherapists)
                               .ThenInclude(st => st.Therapist)
                           .ToListAsync();
        }

        public async Task<Assessment> GetByIdAsync(int id)
        {
            return await _context.Assessments
                           .Include(a => a.schedulerDate)
                               .ThenInclude(s => s.treatment)
                                   .ThenInclude(t => t.ApplicationUser)
                           .Include(a => a.schedulerDate.SchedulerTherapists)
                               .ThenInclude(st => st.Therapist)
                           .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Assessment> UpdateAsync(Assessment entity)
        {
            _context.Assessments.Update(entity);
            await SaveAsync();
            return entity;
        }
    }
}
