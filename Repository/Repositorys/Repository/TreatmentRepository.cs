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
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly ApplicationDbContext _context;
        public TreatmentRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task<treatment> CreateAsync(treatment dto)
        {
            _context.treatments.Add(dto);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.treatments.FindAsync(id);
            if (existing == null)
                return false;

            _context.treatments.Remove(existing);
            return await SaveAsync();
        }

        public async Task<IEnumerable<treatment>> GetAllAsync()
        {
            return await _context.treatments
                           .Include(t => t.Therapist)
                           .Include(t => t.ApplicationUser)
                           .ToListAsync();
        }

        public async Task<treatment> GetByIdAsync(int id)
        {
            return await _context.treatments
                          .Include(t => t.Therapist)
                          .Include(t => t.ApplicationUser)
                          .FirstOrDefaultAsync(t => t.treatmentId == id);
        }

        public async  Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<treatment> UpdateAsync(treatment dto)
        {
            _context.treatments.Update(dto);
            await _context.SaveChangesAsync();
            return dto;
        }
    }
}
