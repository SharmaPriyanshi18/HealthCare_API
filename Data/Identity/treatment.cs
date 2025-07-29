using HealthCareData.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Data.Identity
{
    public class treatment
    {
        [Key]
        public int treatmentId { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public int TherapistId { get; set; }
        public Therapist Therapist { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
