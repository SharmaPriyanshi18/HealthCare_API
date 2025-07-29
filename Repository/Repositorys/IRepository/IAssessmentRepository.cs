using HealthCare_Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repositorys.IRepository
{
    public interface IAssessmentRepository
    {
        Task<IEnumerable<Assessment>> GetAllAsync();
        Task<Assessment> GetByIdAsync(int id);
        Task<Assessment> CreateAsync(Assessment entity);
        Task<Assessment> UpdateAsync(Assessment entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> SaveAsync();
    }
}
