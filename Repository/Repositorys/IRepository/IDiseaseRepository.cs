using HealthCareModels.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository.IRepository
{
    public interface IDiseaseRepository
    {
        Task<IEnumerable<DiseaseDto>> GetAllAsync();
        Task<DiseaseDto> GetByIdAsync(int caseId);
        Task AddAsync(DiseaseDto disease);
        Task UpdateAsync(DiseaseDto disease);
        Task DeleteAsync(int caseId);
        Task SaveAsync();
    }
}
