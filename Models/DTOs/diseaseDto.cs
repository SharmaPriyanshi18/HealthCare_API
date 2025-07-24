using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareModels.Models.DTOs
{
    public class DiseaseDto
    {
        public int CaseId { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public string ApplicationUserId { get; set; }

        public int TherapistId { get; set; }
        public string TherapistName { get; set; }
        public string Address { get; set; }

    }
}
