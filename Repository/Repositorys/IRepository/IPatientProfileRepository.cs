using HealthCareModels.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository.IRepository
{
    public interface IPatientProfileRepository
    {
        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(string id);
        Task AddAsync(PatientDto patient);
        Task UpdateAsync(PatientDto patient);
        Task DeleteAsync(string id);
        Task SaveAsync();
    }
}
