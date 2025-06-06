using Application.ViewModels.PermissionAggregate;
using Domain.Entities;

namespace Application.RepositoryContracts.PermissionAggregate
{
    public interface IPermissionRepository
    {
        Task<PermissionToRoleBaseViewModel> AddPermissionsToRole(PermissionRoleAddCommandViewModel model);
        Task<List<Permission>> GetPermissions();
        Task DeleteRolePermission(long RoleId);
        Task<UpdatePermissionsOfRoleBaseViewModel> UpdatePermissionsOfRole(UpdatePermissionsOfRoleCommandModel model);
        Task<List<RolePermission>> GetPermissionsByRoleName(string roleName);
    }
}