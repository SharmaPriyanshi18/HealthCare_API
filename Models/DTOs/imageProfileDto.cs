using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareModels.Models.DTOs
{
    public class ImageProfileDto
    {
        [Required]
        public IFormFile ImageFile { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
    }
}
