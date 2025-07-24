using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareData.Identity
{
    public class schedulerDate
    {
        [Key]
        public int schedulerId { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public int CaseId { get; set; }
        public Disease Disease { get; set; }
        public ICollection<SchedulerTherapist> SchedulerTherapists { get; set; }
        public bool IsEmailSent { get; set; } = false;
    }
}
