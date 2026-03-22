using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_SOLUTIONS.Services.Implementations
{
    public class AcademicYearService : IAcademicYearService
    {
        private readonly AppDbContext _context;

        public AcademicYearService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AcademicYear>> GetAllAsync()
        {
            return await _context.AcademicYears
                .OrderByDescending(x => x.YearStart)
                .ToListAsync();
        }

        public async Task<AcademicYear?> GetByIdAsync(int id)
        {
            return await _context.AcademicYears.FindAsync(id);
        }

        public async Task AddAsync(AcademicYear model)
        {
            // Ensure only one active year
            if (model.IsActive)
            {
                var activeYears = _context.AcademicYears.Where(x => x.IsActive);
                foreach (var item in activeYears)
                    item.IsActive = false;
            }

            _context.AcademicYears.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AcademicYear model)
        {
            if (model.IsActive)
            {
                var activeYears = _context.AcademicYears.Where(x => x.IsActive);
                foreach (var item in activeYears)
                    item.IsActive = false;
            }

            _context.AcademicYears.Update(model);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> IsDuplicateAsync(string yearName, int id = 0)
        {
            return await _context.AcademicYears
                .AnyAsync(x => x.YearName == yearName && x.AcademicYearID != id);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.AcademicYears.FindAsync(id);
            if (data != null)
            {
                _context.AcademicYears.Remove(data);
                await _context.SaveChangesAsync();
            }
        }
    }
}
