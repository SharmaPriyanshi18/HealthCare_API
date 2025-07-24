using HealthCareModels.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository.IRepository
{
    public interface ITherapistRepository
    {
        Task<IEnumerable<TherapistDto>> GetAllTherapistsAsync();
        Task<TherapistDto> GetTherapistByIdAsync(int therapistId);
        Task<TherapistDto> CreateTherapistAsync(TherapistDto therapistDto);
        Task<TherapistDto> UpdateTherapistAsync(TherapistDto therapistDto);
        Task<bool> DeleteTherapistAsync(int therapistId);
        Task SaveAsync();
    }
}
