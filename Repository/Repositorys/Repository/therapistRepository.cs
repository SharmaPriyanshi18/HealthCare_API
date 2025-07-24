using HealthCareData.Identity;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository
{
    public class TherapistRepository : ITherapistRepository
    {
        private readonly ApplicationDbContext _context;
        public TherapistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TherapistDto> CreateTherapistAsync(TherapistDto therapistDto)
        {
            var therapist = new Therapist
            {
                Name = therapistDto.Name,
                Specialization = therapistDto.Specialization,
                Address = therapistDto.Address,
                PhoneNumber = therapistDto.PhoneNumber
            };

            await _context.Therapists.AddAsync(therapist);
            await SaveAsync();

            therapistDto.TherapistId = therapist.TherapistId;
            return therapistDto;
        }        

        public async Task<bool> DeleteTherapistAsync(int therapistId)
        {
            var therapist = await _context.Therapists.FindAsync(therapistId);

            if (therapist == null) return false;

            _context.Therapists.Remove(therapist);
            await SaveAsync();

            return true;
        }

        public async Task<IEnumerable<TherapistDto>> GetAllTherapistsAsync()
        {
            return await _context.Therapists
                            .Select(t => new TherapistDto
                            {
                                TherapistId = t.TherapistId,
                                Name = t.Name,
                                Specialization = t.Specialization,
                                Address = t.Address,
                                PhoneNumber = t.PhoneNumber
                            })
                            .ToListAsync();
        }

        public async Task<TherapistDto> GetTherapistByIdAsync(int therapistId)
        {
            var therapist = await _context.Therapists.FindAsync(therapistId);

            if (therapist == null) return null;

            return new TherapistDto
            {
                TherapistId = therapist.TherapistId,
                Name = therapist.Name,
                Specialization = therapist.Specialization,
                Address=therapist.Address,
                PhoneNumber=therapist.PhoneNumber
            };
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<TherapistDto> UpdateTherapistAsync(TherapistDto therapistDto)
        {
            var therapist = await _context.Therapists.FindAsync(therapistDto.TherapistId);

            if (therapist == null) return null;

            therapist.Name = therapistDto.Name;
            therapist.Specialization = therapistDto.Specialization;

            _context.Therapists.Update(therapist);
            await SaveAsync();

            return therapistDto;
        }
    }
}
