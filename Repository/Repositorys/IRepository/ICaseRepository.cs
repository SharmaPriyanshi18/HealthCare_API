using HealthCareModels.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository.IRepository
{
    public interface ICaseRepository
    {
        Task<IEnumerable<CaseDto>> GetAllCasesAsync();
        Task<CaseDto> GetCaseByIdAsync(int caseId);
        Task<CaseDto> CreateCaseAsync(CaseDto caseDto);
        Task<CaseDto> UpdateCaseAsync(CaseDto caseDto);
        Task<bool> DeleteCaseAsync(int caseId);
        Task SaveAsync();
    }
}
