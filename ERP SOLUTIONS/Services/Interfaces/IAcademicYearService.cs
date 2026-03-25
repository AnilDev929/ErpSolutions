using ERP_SOLUTIONS.Models.DTOS;
using ERP_SOLUTIONS.Models.Entities;

namespace ERP_SOLUTIONS.Services.Interfaces
{
    public interface IAcademicYearService
    {
        IEnumerable<AcademicYearDropdownDto> GetAll();
        Task<List<AcademicYear>> GetAllAsync();
        Task<AcademicYear?> GetByIdAsync(int id);
        Task AddAsync(AcademicYear model);
        Task UpdateAsync(AcademicYear model);
        Task DeleteAsync(int id);
    }
}
