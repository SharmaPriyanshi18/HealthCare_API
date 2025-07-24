using HealthCareData.Identity;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository
{
    public class ShedularRepository : IShedularRepository
    {
        private readonly ApplicationDbContext _context;

        public ShedularRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<schedulerDate> CreateAsync(schedulerDate entity)
        {
            await _context.schedulers.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var scheduler = await _context.schedulers
                  .Include(s => s.SchedulerTherapists)
                  .FirstOrDefaultAsync(s => s.schedulerId == id);

            if (scheduler == null)
                return false;

            if (scheduler.SchedulerTherapists != null && scheduler.SchedulerTherapists.Any())
            {
                _context.schedulerTherapists.RemoveRange(scheduler.SchedulerTherapists);
            }

            _context.schedulers.Remove(scheduler);

            return true;
        }

        public async Task<IEnumerable<schedulerDate>> GetAllAsync(string includeProperties)
        {
            IQueryable<schedulerDate> query = _context.schedulers;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.ToListAsync();
        }

        public async Task MarkEmailAsSentAsync(int schedulerId)
        {
            var scheduler = await _context.schedulers.FirstOrDefaultAsync(x => x.schedulerId == schedulerId);
            if (scheduler != null)
            {
                scheduler.IsEmailSent = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<schedulerDate> UpdateAsync(schedulerDate date)
        {
            var existingEntity = await _context.schedulers.FindAsync(date.schedulerId);
            if (existingEntity == null)
                return null;

            _context.Entry(existingEntity).CurrentValues.SetValues(date);
            return existingEntity;
        }
    }
}
