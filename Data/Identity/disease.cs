using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareData.Identity
{
    public class Disease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
           public int CaseId { get; set; }

            public string Title { get; set; }

            public DateTime DateCreated { get; set; }

            public string ApplicationUserId { get; set; }

            public int TherapistId { get; set; }

            public ApplicationUser ApplicationUser { get; set; }
            public Therapist Therapist { get; set; }
        }
    }


