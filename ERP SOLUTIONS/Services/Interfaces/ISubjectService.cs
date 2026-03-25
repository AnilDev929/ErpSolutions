using ERP_SOLUTIONS.Models.Entities;

namespace ERP_SOLUTIONS.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<bool> AddSubjectAsync(Subject subject);
        Task<List<Subject>> GetAllSubjectsAsync();
        Task<Subject> GetSubjectByIdAsync(int id);
        Task<bool> UpdateSubjectAsync(Subject subject);
        Task<bool> DeleteSubjectAsync(int id);
    }
}
