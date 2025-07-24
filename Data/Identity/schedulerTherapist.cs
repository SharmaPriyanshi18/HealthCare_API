using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareData.Identity
{
    public class SchedulerTherapist
    {
        [Key]
        public int Id { get; set; }

        public int SchedulerId { get; set; }
        [ForeignKey("SchedulerId")]
        public schedulerDate Scheduler { get; set; }

        public int TherapistId { get; set; }
        [ForeignKey("TherapistId")]
        public Therapist Therapist { get; set; }
    }
}
