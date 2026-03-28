using ERP_SOLUTIONS.Models.DTOS;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Models.ViewModels;

namespace ERP_SOLUTIONS.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<RolePermissionViewModel> GetRolePermissionsAsync(int roleId);
        Task SaveRolePermissionsAsync(RolePermissionViewModel model);
        Task<List<Role>> GetAllRolesAsync();
    }
}
