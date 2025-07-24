using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareModels.Models.DTOs
{
    public class CaseDto
    {
        public int CaseId { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public string PatientId { get; set; }

        public int TherapistId { get; set; }
    }
}
