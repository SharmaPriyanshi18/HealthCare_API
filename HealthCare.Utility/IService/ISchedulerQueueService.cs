using HealthCareModels.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository.IRepository
{
    public interface ISchedulerQueueService
    {
        Task EnqueueAsync(schedularDto dto);

    }
}
