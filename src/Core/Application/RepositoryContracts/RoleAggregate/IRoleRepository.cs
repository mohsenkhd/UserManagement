using Application.ViewModels.RoleAggregate;
using Application.ViewModels.UserAggregate;
using Domain.Entities;
using System.Reflection;

namespace Application.RepositoryContracts.RoleAggregate
{
    public interface IRoleRepository
    {
        Task<RoleToUserBaseViewModel> AddRolesToUser(RoleToUserAddCommandModel model);
        Task<Role> AddRole(Role command);
        Task<List<Role>> GetRoles();
        Task<bool> IsRoleExistByName(string name);
        Task<bool> DeleteRole(long RoleId);
        Task<Role> GetRoleById(long RoleId);
        Task<Role> UpdateRole(Role model);
        //Task<bool> IsRoleDeleted(long RoleId);
        Task DeleteUsersRole(long RoleId);
        Task DeleteRolePermissions(long RoleId);
        Task<bool> IsUserAdministrator(long UserId);
        Task<UpdateRolesOfUserBaseViewModel> UpdateRolesOfUser( UpdateRolesOfUserCommandModel model);
        Task<bool> RolesExist(List<long>roleIds);
        Task<(int, IQueryable<Role>)> FilterAllAsync(RoleFilterCommandModel model);
        Task<GetRolesBaseViewModel> GethRolsWithPermissions(int page, int pageSize, IQueryable<Role> roles);
        Task<List<Role>> GetRolesSearch(string? roleName);
        Task<Role> GetRoleWithPermissionsById(long RoleId);
        Task ActiveInactiveRole(ActiveDeductiveRoleCommandModel model);
        Task<RoleToUserBaseViewModel> AddNormalUserRoleToUser(RoleToUserAddCommandModel model);



    }
}