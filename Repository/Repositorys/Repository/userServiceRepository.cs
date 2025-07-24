using HealthCareData.Identity;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HealthCare_API;
using HealthCareData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace HealthCareRepositorys.Repository
{
    public class userServiceRepository : IUserServiceRepository
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly AppSettings _appSettings; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public userServiceRepository(
        ApplicationUserManager applicationUserManager,
        IOptions<AppSettings> appSettings,IConfiguration configuration)
        {
            _applicationUserManager = applicationUserManager;
            _appSettings = appSettings.Value;
            _configuration = configuration;

        }

        public async Task<LoginResponseDto> Authenticate(loginDto loginVM)
        {
            var user = await _applicationUserManager.FindByNameAsync(loginVM.UserName);

            if (user == null || !await _applicationUserManager.CheckPasswordAsync(user, loginVM.Password))
            {
                return null;
            }


            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var roles = await _applicationUserManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Role = roles.FirstOrDefault() ?? "",
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}



