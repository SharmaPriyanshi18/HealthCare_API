using HealthCareData.Identity;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthCare_API.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserServiceRepository _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IUserServiceRepository userService, SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] loginDto loginVM)
        {
            var loginResponse = await _userService.Authenticate(loginVM);
            if (loginResponse == null)
                return BadRequest("Invalid username or password");

            return Ok(new
            {
                Message = "User authenticated successfully",
                Token = loginResponse.Token,
                Role = loginResponse.Role,
                Id = loginResponse.Id,
                UserName = loginResponse.UserName,
                Email = loginResponse.Email,
                Password = loginVM.Password
            });
        }
    }
}

