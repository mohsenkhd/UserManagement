using Application.ViewModels.AccountAggregate;
using Application.ViewModels.RoleAggregate;
using Domain.Entities;

namespace Application.ServiceContracts.RoleAggregate
{
    public interface IRoleService
    {
        Task<RoleToUserBaseViewModel> AddRolesToUser(RoleToUserAddCommandModel model);
        Task<RoleBaseViewModel> AddRole(RoleAddCommandModel command);
        Task<List<RoleBaseViewModel>> GetRoles(GetRolesAddCommandModel model);
        Task<DeleteRoleBaseViewModel> DeleteRole(long roleId);
        Task<UpdateRoleBaseViewModel> UpdateRole(UpdateRoleCommandModel model);
        Task<UpdateRolesOfUserBaseViewModel> UpdateRolesOfUser(UpdateRolesOfUserCommandModel model);
        Task<Role> GetRoleById(long RoleId);
        Task<GetRolesBaseViewModel> GetRolesWithPermissions(GetRolesAddCommandModel model);
        Task<ActiveDeductiveRoleViewModel> ActiveInactiveRole(ActiveDeductiveRoleCommandModel model);
        Task<GetRolesSearchViewModel> GetRolesSearch(GetRolesSearchAddCommandModel model);
        Task<GetRoleWithPermissionsByIdBaseViewModel> GetRoleWithPermissionsById(GetRoleWithPermissionsByIdCommandModel model);
    }
}