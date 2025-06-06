using Application.RepositoryContracts.RoleAggregate;
using Application.RepositoryContracts.UserAggregate;
using Application.ServiceContracts.PermissionService;
using Application.ServiceContracts.RoleAggregate;
using Application.ServiceContracts.UserAggregate;
using Application.ViewModels.PermissionAggregate;
using Application.ViewModels.RoleAggregate;
using Application.ViewModels.UserAggregate;
using Common.Exceptions.UserManagement;
using Domain.Entities;

namespace Service.RoleAggregate
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionService _permissionService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public RoleService(IJwtService jwtService, IRoleRepository roleRepository, IPermissionService permissionService, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _permissionService = permissionService;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<RoleBaseViewModel> AddRole(RoleAddCommandModel command)
        {
            var roleExist = await _roleRepository.IsRoleExistByName(command.Name);
            if (roleExist)
            {

                throw UserManagementExceptions.RoleNameExistException;
            }

            var role = new Role()
            {
                IsDeleted = false,
                Name = command.Name,
                IsActive = true,
                Description = command.Description,
            };
            var res = await _roleRepository.AddRole(role);
            var permissionToRoleModel = new PermissionRoleAddCommandViewModel()
            {
                PermissionIds = command.PermissionsIds,
                RoleId = res.Id
            };
            var permissionToRole = await _permissionService.AddPermissionsToRole(permissionToRoleModel);
            if (!permissionToRole.IsSuccess)
            {

                throw UserManagementExceptions.CantAssignPermissionsToRoleException;
            }

            return new RoleBaseViewModel()
            {
                RoleId = (int)res.Id,
                Description = res.Description,
                Name = command.Name,
            };
        }

        public async Task<RoleToUserBaseViewModel> AddRolesToUser(RoleToUserAddCommandModel model)
        {
            return await _roleRepository.AddRolesToUser(model);
        }

        public async Task<DeleteRoleBaseViewModel> DeleteRole(long roleId)
        {
            if (roleId == 3)
            {
                throw UserManagementExceptions.RoleUpdateException;
            }
            var res = await _roleRepository.DeleteRole(roleId);
            if (res)
            {
                await _roleRepository.DeleteUsersRole(roleId);
                await _permissionService.DeleteRolePermission(roleId);

                return new DeleteRoleBaseViewModel()
                {
                    IsSuccess = true,
                };
            }
            return new DeleteRoleBaseViewModel()
            {
                IsSuccess = false,
            };
        }



        public async Task<List<RoleBaseViewModel>> GetRoles(GetRolesAddCommandModel model)
        {
            var res = new List<RoleBaseViewModel>();
            var roles = await _roleRepository.GetRoles();
            foreach (var item in roles)
            {
                res.Add((RoleBaseViewModel)item);
            }

            return res;
        }

        public async Task<UpdateRoleBaseViewModel> UpdateRole(UpdateRoleCommandModel model)
        {
            var role = new Role()
            {
                Name = model.Name,
                Description = model.Description,
                Id=model.RoleId
            };
            var updateRole = await _roleRepository.UpdateRole(role);
            var updatPermissionsOfRoleModel = new UpdatePermissionsOfRoleCommandModel()
            {
                PermissionsIds = model.PermissionsIds,
                RoleId = model.RoleId,
            };

            await _permissionService.UpdatePermissionsOfRole(updatPermissionsOfRoleModel);
            return new UpdateRoleBaseViewModel()
            {
                Name = updateRole.Name,
                Description = updateRole.Description,
            };
        }

        public async Task<UpdateRolesOfUserBaseViewModel> UpdateRolesOfUser(UpdateRolesOfUserCommandModel model)
        {
            var user = await _userRepository.GetById(model.UserId);
            if (user == null)
            {
                throw UserManagementExceptions.UserNotFoundException;
            }

            var rolesExist = await _roleRepository.RolesExist(model.RoleIds);
            if (!rolesExist)
            {
                throw UserManagementExceptions.RolesNotFoundException;
            }

            return await _roleRepository.UpdateRolesOfUser(model);
        }
        public async Task<Role> GetRoleById(long roleId)
        {
            return await _roleRepository.GetRoleById(roleId);
        }

        public async Task<GetRolesBaseViewModel> GetRolesWithPermissions(GetRolesAddCommandModel model)
        {
            var filterAllModel = new RoleFilterCommandModel()
            {
                RoleNames = model.Names,
                IsActive = model.IsActive,
            };

            var count = await _roleRepository.FilterAllAsync(filterAllModel);
            var rolesPermissions = await _roleRepository.GethRolsWithPermissions(model.Page, model.PerPage, count.Item2);

            var res = new GetRolesBaseViewModel()
            {
                Elements = rolesPermissions.Elements,
                TotalElements = count.Item1,
                TotalPages = (int)Math.Ceiling((decimal)count.Item1 / model.PerPage),
                HasNext = (model.Page + 1) < (int)Math.Ceiling((decimal)count.Item1 / model.PerPage),
                HasPrev = model.Page > 0,
                Page = model.Page,
            };

            return res;
        }

        public async Task<ActiveDeductiveRoleViewModel> ActiveInactiveRole(ActiveDeductiveRoleCommandModel model)
        {
            await _roleRepository.ActiveInactiveRole(model);
            await _roleRepository.DeleteUsersRole(model.RoleId);
            return new ActiveDeductiveRoleViewModel()
            {
                IsSucced = true,
            };
        }

        public async Task<GetRolesSearchViewModel> GetRolesSearch(GetRolesSearchAddCommandModel model)
        {
            var res = new List<GetRolesSearch>();
            var roles = await _roleRepository.GetRolesSearch(model.RoleName);
            foreach (var role in roles)
            {
                res.Add(new GetRolesSearch
                {
                    RoleName = role.Name,
                    RoleId = role.Id,
                });
            }
            return new GetRolesSearchViewModel
            {
                Roles = res
            };
        }

        public async Task<GetRoleWithPermissionsByIdBaseViewModel> GetRoleWithPermissionsById(GetRoleWithPermissionsByIdCommandModel model)
        {
            var role = await _roleRepository.GetRoleWithPermissionsById(model.RoleId);

            var res = new GetRoleWithPermissionsByIdBaseViewModel()
            {
                RoleName = role.Name,
                RoleDescription = role.Description,
                IsActive = role.IsActive,
                Permissions = role.RolePermissions.Select(rp => new PermissionResultViewModel
                {
                    PermissionId = rp.PermissionId,
                    Title = rp.Permission.Title,
                    ParentId = rp.Permission.ParentId
                }).ToList()
            };
            return res;
        }
    }
}