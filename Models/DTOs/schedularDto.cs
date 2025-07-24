using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareModels.Models.DTOs
{
    public class schedularDto
    {
 
        public int SchedulerId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int CaseId { get; set; }
        public string Title { get; set; }
        public string ApplicationUserId { get; set; }
        public string UserName { get; set; }
        public string address { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }

        public string TherapistIds { get; set; }
        public string TherapistNames { get; set; }
    }
}




