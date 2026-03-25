using ERP_SOLUTIONS.Models.DTOS;

namespace ERP_SOLUTIONS.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<List<TeacherDto>> GetAllAsync();
    }
}
