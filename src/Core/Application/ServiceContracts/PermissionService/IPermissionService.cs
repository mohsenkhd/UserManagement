using Application.ViewModels.PermissionAggregate;

namespace Application.ServiceContracts.PermissionService
{
    public interface IPermissionService
    {
        Task<PermissionToRoleBaseViewModel> AddPermissionsToRole(PermissionRoleAddCommandViewModel model);
        Task<PermissionBaseViewModel> GetAllPermissions();
        Task DeleteRolePermission(long RoleId);
        Task<UpdatePermissionsOfRoleBaseViewModel> UpdatePermissionsOfRole(UpdatePermissionsOfRoleCommandModel model);
        Task<PermissionByRoleNameViewModel> GetPermissionsByRoleName(string roleName);
    }
}