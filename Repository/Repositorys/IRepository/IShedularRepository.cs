using HealthCareData.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository.IRepository
{
    public interface IShedularRepository
    {
        Task<IEnumerable<schedulerDate>> GetAllAsync(string includeProperties);
        Task<schedulerDate> CreateAsync(schedulerDate entity);
        Task<schedulerDate> UpdateAsync(schedulerDate entity);
        Task<bool> DeleteAsync(int id);
        Task MarkEmailAsSentAsync(int schedulerId);
        Task SaveAsync();
    }
}
