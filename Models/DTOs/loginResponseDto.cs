using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareModels.Models.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

    }
}
