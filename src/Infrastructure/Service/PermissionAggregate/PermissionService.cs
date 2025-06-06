using Application.RepositoryContracts.PermissionAggregate;
using Application.RepositoryContracts.RoleAggregate;
using Application.ServiceContracts.PermissionService;
using Application.ServiceContracts.RoleAggregate;
using Application.ViewModels.PermissionAggregate;
using Common.Exceptions.UserManagement;

namespace Service.PermissionAggregate
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;

        public PermissionService(IPermissionRepository permissionRepository, IRoleRepository roleRepository)
        {
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
        }

        public async Task<PermissionToRoleBaseViewModel> AddPermissionsToRole(PermissionRoleAddCommandViewModel model)
        {
            var res = await _permissionRepository.AddPermissionsToRole(model);
            return res;
        }



        public async Task<PermissionBaseViewModel> GetAllPermissions()
        {
            var permissions = await _permissionRepository.GetPermissions();

            var permissionResultViewModels = permissions.Select(p => new PermissionResultViewModel
            {
                PermissionId = p.Id,
                Title = p.Title,
                ParentId = p.ParentId
            }).ToList();

            var permissionBaseViewModel = new PermissionBaseViewModel
            {
                Permissions = permissionResultViewModels
            };

            return permissionBaseViewModel;
        }
        public async Task DeleteRolePermission(long RoleId)
        {
            await _permissionRepository.DeleteRolePermission(RoleId);
        }

        public async Task<UpdatePermissionsOfRoleBaseViewModel> UpdatePermissionsOfRole(UpdatePermissionsOfRoleCommandModel model)
        {
            var role = await _roleRepository.GetRoleById(model.RoleId);
            if (role == null)
            {
                throw UserManagementExceptions.RoleNotFoundException;
            }

            return await _permissionRepository.UpdatePermissionsOfRole(model);
        }

        public async Task<PermissionByRoleNameViewModel> GetPermissionsByRoleName(string roleName)
        {
            var permissions=await _permissionRepository.GetPermissionsByRoleName(roleName);
            var permissionIds=permissions.Select(p => p.PermissionId).ToList();
            return new PermissionByRoleNameViewModel()
            {
                Permissions=permissionIds
            };
        }
    }
}