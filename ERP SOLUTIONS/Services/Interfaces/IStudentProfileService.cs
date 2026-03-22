using ERP_SOLUTIONS.Models.Entities;

namespace ERP_SOLUTIONS.Services.Interfaces
{
    public interface IStudentProfileService
    {
        Task<Student> GetStudentProfileAsync(int StudentID);
        Task UpdateStudentProfileAsync(Student model);
    }
}
