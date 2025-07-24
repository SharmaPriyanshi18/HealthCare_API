using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareData.Identity
{
   public class ImageProfile
    {
        [Key]
        public int Id { get; set; }

        public byte[]? ImageData { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public  ApplicationUser ApplicationUser { get; set; }

    }
}
