using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Enum;
using ERP_SOLUTIONS.Models.DTOS;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

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

        public async Task<IEnumerable<TeacherDto>> GetAllAsync()
        {
            // Join with Gender table to get Gender name
            var teachers = await (from t in _context.Teachers
                                  join g in _context.Genders on t.GenderID equals g.GenderID
                                  select new TeacherDto
                                  {
                                      TeacherID = t.TeacherID,
                                      TeacherName = t.FullName,
                                      Designation = t.Designation,
                                      Qualification = t.Qualification,
                                      Specialization = t.Specialization,
                                      Gender = g.GenderName,
                                      Phone = t.Phone ?? string.Empty,
                                      EmailID = t.EmailID ?? "N/A",
                                  }).ToListAsync();
            return teachers;

        }

        public async Task<Teacher> GetByUserIdAsync(int userId)
        {
            return await _context.Teachers
                .FirstOrDefaultAsync(t => t.UserID == userId);
        }

        public async Task<Teacher> GetByIdAsync(int teacherId)
        {
            return await _context.Teachers.FindAsync(teacherId);
        }

        public async Task<(string username, string password)> CreateTeacherWithUserAsync(Teacher teacher)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var exists = await _context.Teachers.AnyAsync(t =>
                    t.Phone == teacher.Phone &&
                    t.EmailID == teacher.EmailID &&
                    t.DateOfBirth == teacher.DateOfBirth);

                if (exists)
                {
                    return("", "Teacher already exists.");
                }

                // 1️⃣ Generate unique username based on count
                var username = await GenerateUniqueUsernameAsync("FAC");

                // 2️⃣ Create User
                var user = new User
                {
                    UserName = username,
                    Email = teacher.EmailID,
                    PasswordHash = HashPassword("Password@123"), // default password
                    CreatedAt = DateTime.Now,
                    LastLogin = null,      // if optional
                    LockoutUntil = null,    // if optional
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // get UserID

                // 3️⃣ Link Teacher to User
                teacher.UserID = user.UserID;
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return (username, "Password@123");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<(bool Status, string message)> UpdateAsync(Teacher teacher)
        {
            var exists = await _context.Teachers.AnyAsync(t =>
                t.Phone == teacher.Phone &&
                t.EmailID == teacher.EmailID &&
                t.DateOfBirth == teacher.DateOfBirth &&
                t.TeacherID != teacher.TeacherID);

            if (exists)
            {
                return (false, "Another teacher with same details exists.");
            }

            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
            return (true, "Teacher detail updated successfully.");
        }

        public async Task DeactivateAsync(int teacherId)
        {
            var teacher = await _context.Teachers.FindAsync(teacherId);
            if (teacher != null)
            {
                teacher.Status = TeacherStatus.Blocked;
                teacher.LastWorkingDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }
        
        private string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }

        //Generate username based on current role count
        private async Task<string> GenerateUniqueUsernameAsync(string rolePrefix)
        {
            // Count how many users already exist for this role
            var count = await _context.Users
                .Where(u => u.UserName.StartsWith(rolePrefix))
                .CountAsync();

            int nextNumber = count + 1;

            // Dynamically determine number of digits
            int digits = nextNumber.ToString().Length; // auto adjust

            return $"{rolePrefix}_{nextNumber.ToString($"D{digits}")}";
        }

        // Teacher Methods
        public async Task<TeacherProfileDTO> GetProfileByUserIdAsync(int userId)
        {
            var profile = await (from t in _context.Teachers
                                 join g in _context.Genders on t.GenderID equals g.GenderID
                                 where t.UserID == userId
                                 select new TeacherProfileDTO
                                 {
                                     TeacherID = t.TeacherID,
                                     FullName = t.FullName,
                                     GenderName = g.GenderName,   // direct join
                                     DateOfBirth = t.DateOfBirth,
                                     BloodGroup = t.BloodGroup,
                                     FatherName = t.FatherName,
                                     MotherName = t.MotherName,
                                     IsMarried = t.IsMarried,
                                     SpouseName = t.SpouseName,
                                     SpouseContact = t.SpouseContact,
                                     Phone = t.Phone,
                                     EmailID = t.EmailID,
                                     EmergencyContact = t.EmergencyContact,
                                     PresentAddress = t.PresentAddress,
                                     PermanentAddress = t.PermanentAddress,
                                     Qualification = t.Qualification,
                                     Specialization = t.Specialization,
                                     Experience = t.Experience
                                 }).FirstOrDefaultAsync();

            return profile;
        }

        public async Task<bool> UpdateProfileAsync(TeacherProfileDTO model, int userId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserID == userId);
            if (teacher == null) return(false);

            // Update only allowed fields
            teacher.FullName = model.FullName;
            teacher.BloodGroup = model.BloodGroup;
            teacher.IsMarried = model.IsMarried;
            teacher.SpouseName = model.SpouseName;
            teacher.SpouseContact = model.SpouseContact;
            teacher.Phone = model.Phone;
            teacher.EmailID = model.EmailID;
            teacher.EmergencyContact = model.EmergencyContact;
            teacher.PresentAddress = model.PresentAddress;
            teacher.PermanentAddress = model.PermanentAddress;
            teacher.Qualification = model.Qualification;
            teacher.Specialization = model.Specialization;
            teacher.Experience = model.Experience;

            await _context.SaveChangesAsync();
            return(true);
        }
    }
}
