using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Models.DTOs
{
        public class AssessmentDto
        {
            public int AssessmentId { get; set; }

            public int SchedulerId { get; set; }

            public string PatientName { get; set; }

            public string TherapistName { get; set; }

            public string PhoneNumber { get; set; }

            public string Treatment { get; set; }

            public DateTime ScheduleDate { get; set; }
        }

    }

