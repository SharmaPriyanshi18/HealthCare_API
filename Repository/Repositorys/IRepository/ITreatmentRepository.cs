using HealthCare_Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repositorys.IRepository
{
    public interface ITreatmentRepository
    {
    
        
            Task<IEnumerable<treatment>> GetAllAsync();
            Task<treatment> GetByIdAsync(int id);
            Task<treatment> CreateAsync(treatment dto);
            Task<treatment> UpdateAsync(treatment dto);
            Task<bool> DeleteAsync(int id);
            Task<bool> SaveAsync();
        }
    }


