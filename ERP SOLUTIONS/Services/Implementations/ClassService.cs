using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_SOLUTIONS.Services.Implementations
{
    public class ClassService : IClassService
    {
        private readonly AppDbContext _context;

        public ClassService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SchoolClass>> GetAllAsync()
        {
            return await _context.SchoolClasses.ToListAsync();
        }

        public async Task<SchoolClass?> GetByIdAsync(int id)
        {
            return await _context.SchoolClasses
                .FirstOrDefaultAsync(c => c.ClassID == id);
        }

        public async Task<bool> AddAsync(SchoolClass cls)
        {
            bool exists = await _context.SchoolClasses
                .AnyAsync(c => c.ClassName == cls.ClassName && c.Stream == cls.Stream);

            if (exists)
            {
                return false; // Already exists
            }

            await _context.SchoolClasses.AddAsync(cls);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(SchoolClass cls)
        {
            bool exists = await _context.SchoolClasses
                .AnyAsync(c =>
                    c.ClassID != cls.ClassID && // exclude current record
                    c.ClassName == cls.ClassName &&
                    c.Stream == cls.Stream);

            if (exists)
            {
                return false; // Duplicate found
            }

            _context.SchoolClasses.Update(cls);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task DeleteAsync(int id)
        {
            var cls = await _context.SchoolClasses.FindAsync(id);
            if (cls != null)
            {
                _context.SchoolClasses.Remove(cls);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SchoolClasses.AnyAsync(e => e.ClassID == id);
        }
    }
}
