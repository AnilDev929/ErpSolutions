using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_SOLUTIONS.Services.Implementations
{
    public class StudentProfileService : IStudentProfileService
    {

        private readonly AppDbContext _context;
        private readonly ILogger<RoleService> _logger;

        public StudentProfileService(AppDbContext context, ILogger<RoleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Student> GetStudentProfileAsync(int id)
        {
            var student = await _context.Students
            .Where(s => s.StudentID == id && s.IsActive)
            .FirstOrDefaultAsync();

            return student ?? new Student(); // Return empty Student if not found
        }

        public async Task UpdateStudentProfileAsync(Student model)
        {
            var student = await _context.Students.FindAsync(model.StudentID);
            if (student == null) throw new Exception("Student not found");

            // Update fields
            student.FullName = model.FullName;
            student.Gender = model.Gender;
            student.MobileNumber = model.MobileNumber;
            student.Email = model.Email;
            student.DateOfBirth = model.DateOfBirth;
            student.DateOfJoin = model.DateOfJoin;
            student.PermanentAddress = model.PermanentAddress;
            student.ParentName = model.ParentName;
            student.ParentContact = model.ParentContact;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
