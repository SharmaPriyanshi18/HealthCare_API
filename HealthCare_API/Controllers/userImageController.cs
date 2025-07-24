using HealthCareData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using HealthCareModels.Models.DTOs;

namespace HealthCare_API.Controllers
{
    [Route("api/Image")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImageController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageProfileDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
                return BadRequest("No image file provided.");

            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}_{dto.ImageFile.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ImageFile.CopyToAsync(fileStream);
            }

            var userImage = new ImageProfile
            {
                ApplicationUserId = dto.ApplicationUserId,
                FileName = uniqueFileName,
                FilePath = Path.Combine("uploads", uniqueFileName)
            };

            _context.ImageProfiles.Add(userImage);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Image uploaded successfully.", filePath = userImage.FilePath });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetImage(string userId)
        {
            var image = await _context.ImageProfiles
                .FirstOrDefaultAsync(i => i.ApplicationUserId == userId);

            if (image == null)
                return NotFound("Image not found.");

            var filePath = Path.Combine(_env.WebRootPath, image.FilePath);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on server.");

            var contentType = "application/octet-stream";
            return PhysicalFile(filePath, contentType, image.FileName);
        }
    }
}
