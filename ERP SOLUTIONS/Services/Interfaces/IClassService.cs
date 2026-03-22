using ERP_SOLUTIONS.Models.Entities;

namespace ERP_SOLUTIONS.Services.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<SchoolClass>> GetAllAsync();
        Task<SchoolClass?> GetByIdAsync(int id);
        Task<bool> AddAsync(SchoolClass cls);
        Task<bool> UpdateAsync(SchoolClass cls);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
