using AutoMapper;
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
    public class CaseRepository : ICaseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CaseRepository(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CaseDto> CreateCaseAsync(CaseDto caseDto)
        {
            var caseEntity = _mapper.Map<Case>(caseDto);
            await _context.Cases.AddAsync(caseEntity);
            await SaveAsync();
            return _mapper.Map<CaseDto>(caseEntity);
        }

        public async Task<bool> DeleteCaseAsync(int caseId)
        {
            var caseEntity = await _context.Cases.FindAsync(caseId);
            if (caseEntity == null) return false;

            _context.Cases.Remove(caseEntity);
            await SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CaseDto>> GetAllCasesAsync()
        {
            var cases = await _context.Cases.ToListAsync();
            return _mapper.Map<IEnumerable<CaseDto>>(cases);
        }

        public async Task<CaseDto> GetCaseByIdAsync(int caseId)
        {
            var caseEntity = await _context.Cases.FindAsync(caseId);
            return _mapper.Map<CaseDto>(caseEntity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<CaseDto> UpdateCaseAsync(CaseDto caseDto)
        {
            var existing = await _context.Cases.FindAsync(caseDto.CaseId);
            if (existing == null) return null;

            _mapper.Map(caseDto, existing);
            _context.Cases.Update(existing);
            await SaveAsync();
            return _mapper.Map<CaseDto>(existing);
        }
    }
}
