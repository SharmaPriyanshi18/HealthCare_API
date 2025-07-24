using HealthCareData.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareModels.Models.DTOs
{
    public class TherapistDto
    {
        public int TherapistId { get; set; }

        public string Name { get; set; }

        public string Specialization { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
