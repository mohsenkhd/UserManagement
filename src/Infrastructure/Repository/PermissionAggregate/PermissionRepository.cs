using Application.RepositoryContracts.PermissionAggregate;
using Application.ViewModels.PermissionAggregate;
using Application.ViewModels.RoleAggregate;
using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.PermissionAggregate
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly UserManagementContext _context;

        public PermissionRepository(UserManagementContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<PermissionToRoleBaseViewModel> AddPermissionsToRole(PermissionRoleAddCommandViewModel model)
        {
            foreach (var p in model.PermissionIds)
            {
                await _context.RolePermissions.AddAsync(new RolePermission()
                {
                    PermissionId = p,
                    RoleId = model.RoleId
                });
            }
            await _context.SaveChangesAsync();
            return new PermissionToRoleBaseViewModel()
            {
                IsSuccess = true
            };
        }

    

        public async Task<List<Permission>> GetPermissions()
        {
            return await _context.Permissions.Where(a=>a.ForClient==false).ToListAsync();
        }
        public async Task DeleteRolePermission(long RoleId)
        {
            var deletedrole = await _context.RolePermissions.Where(a => a.RoleId == RoleId).ToListAsync();
            _context.RemoveRange(deletedrole);
            await _context.SaveChangesAsync();
        }
        public async Task<UpdatePermissionsOfRoleBaseViewModel> UpdatePermissionsOfRole(UpdatePermissionsOfRoleCommandModel model)
        {
            _context.RolePermissions.Where(p => p.RoleId == model.RoleId)
               .ToList().ForEach(p => _context.RolePermissions.Remove(p));
            var permissionsToRoleModel = new PermissionRoleAddCommandViewModel()
            {
                RoleId = model.RoleId,
                PermissionIds = model.PermissionsIds,
            };
            await AddPermissionsToRole(permissionsToRoleModel);
            return new UpdatePermissionsOfRoleBaseViewModel()
            {
                IsSuccess = true,
            };
        }

        public async Task<List<RolePermission>> GetPermissionsByRoleName(string roleName)
        {
            var permissions = await _context.RolePermissions.Where(rp => rp.Role.Name == roleName).ToListAsync();
            return permissions;
        }
    }
}