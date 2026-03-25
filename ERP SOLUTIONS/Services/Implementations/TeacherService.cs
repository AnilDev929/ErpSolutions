using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Models.DTOS;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_SOLUTIONS.Services.Implementations
{
    public class TeacherService : ITeacherService
    {

        private readonly AppDbContext _context;
        private readonly ILogger<SubjectService> _logger;

        public TeacherService(AppDbContext context, ILogger<SubjectService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TeacherDto>> GetAllAsync()
        {
            try
            {
                return await _context.Teacher
                .Select(t => new TeacherDto
                {
                    TeacherID = t.TeacherID,
                    Name = t.FullName
                })
                .OrderBy(t => t.Name)
                .ToListAsync();
            }
            catch(Exception)
            {
                throw;
            }
            
        }
    }
}
