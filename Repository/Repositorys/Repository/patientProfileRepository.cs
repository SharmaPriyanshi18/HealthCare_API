using AutoMapper;
using HealthCareData.Identity;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository
{
    public class PatientProfileRepository : IPatientProfileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public PatientProfileRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task AddAsync(PatientDto patient)
        {
            var password = patient.Password;

            var user = new ApplicationUser
            {
                UserName = patient.UserName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address,
                City = patient.City,
                State = patient.State,
                Country = patient.Country,
                PostalCode = patient.PostalCode
            };

            var result = await _userManager.CreateAsync(user, password); 
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Patient");
            }
        }
        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new PatientDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                City = u.City,
                State = u.State,
                Country = u.Country,
                PostalCode = u.PostalCode,
                Password=u.PasswordHash
                
            });
        }

        public async Task<PatientDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return new PatientDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                City = user.City,
                State = user.State,
                Country = user.Country,
                PostalCode = user.PostalCode
            };
        }

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PatientDto patient)
        {
            var user = await _userManager.FindByIdAsync(patient.Id.ToString());
            if (user == null) return;

            user.UserName = patient.UserName;
            user.Email = patient.Email;
            user.PhoneNumber = patient.PhoneNumber;
            user.Address = patient.Address;
            user.City = patient.City;
            user.State = patient.State;
            user.Country = patient.Country;
            user.PostalCode = patient.PostalCode;
            await _userManager.UpdateAsync(user);
        }
    }

}

