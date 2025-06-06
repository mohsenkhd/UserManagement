using Application.RepositoryContracts.RoleAggregate;
using Application.ViewModels.RoleAggregate;
using Domain.Entities;
using Context.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Common.Exceptions.UserManagement;
using Application.ViewModels.UserAggregate;
using Common.Pagination;
using System.Data;

namespace Repository.RoleAggregate
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserManagementContext _context;
        IQueryable<Role> AllRoles;

        public RoleRepository(UserManagementContext dbContext)
        {
            _context = dbContext;
            AllRoles = dbContext.Roles;
        }

        public async Task<Role> AddRole(Role command)
        {
            await _context.Roles.AddAsync(command);

            await _context.SaveChangesAsync();

            return command;
        }

        public async Task<RoleToUserBaseViewModel> AddRolesToUser(RoleToUserAddCommandModel model)
        {

            if (model.RoleIds.Contains(3))
            {
                throw UserManagementExceptions.RoleUpdateException;
            }
            foreach (var roleId in model.RoleIds)
            {
                await _context.UserRoles.AddAsync(new UserRole()
                {
                    RoleId = roleId,
                    UserId = model.UserId
                });
            }
            await _context.SaveChangesAsync();

            return new RoleToUserBaseViewModel()
            {
                IsSucced = true,
            };
        }
        public async Task<RoleToUserBaseViewModel> AddNormalUserRoleToUser(RoleToUserAddCommandModel model)
        {
            foreach (var roleId in model.RoleIds)
            {
                await _context.UserRoles.AddAsync(new UserRole()
                {
                    RoleId = roleId,
                    UserId = model.UserId
                });
            }
            await _context.SaveChangesAsync();

            return new RoleToUserBaseViewModel()
            {
                IsSucced = true,
            };
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.Where(a => a.IsDeleted == false).ToListAsync();
        }

        public async Task<bool> IsRoleExistByName(string name)
        {
            return await _context.Roles.AnyAsync(a => a.Name == name && a.IsDeleted == false);
        }

        public async Task<bool> DeleteRole(long roleId)
        {
            var role = await GetRoleById(roleId);
            if (role == null) throw UserManagementExceptions.RoleNotFoundException;
            role.IsDeleted = true;
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Role> GetRoleById(long roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(a => a.Id == roleId && !a.IsDeleted);
            return role ?? throw UserManagementExceptions.RoleNotFoundException;
        }

        public async Task<Role> UpdateRole(Role model)
        {
            var role = await GetRoleById(model.Id);
            if (role == null) throw UserManagementExceptions.RoleNotFoundException;
            var roleExist = await IsRoleExistByName(model.Name);
            if (!roleExist)
            {
                var roleExistByName = await IsRoleExistByName(model.Name);
                if (roleExistByName)
                {
                    throw UserManagementExceptions.RoleNameExistException;
                }
                role.Name = model.Name;
                role.Description = model.Description;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                return role;
            }
            if (roleExist && model.Id == role.Id)
            {
                role.Name = model.Name;
                role.Description = model.Description;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                return role;
            }
            throw UserManagementExceptions.RoleNotFoundException;
        }

        public async Task DeleteUsersRole(long roleId)
        {
            var deletedRole = await _context.UserRoles.Where(a => a.RoleId == roleId).ToListAsync();
            _context.RemoveRange(deletedRole);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRolePermissions(long roleId)
        {
            var deletedRole = await _context.RolePermissions.Where(a => a.RoleId == roleId).ToListAsync();
            _context.RemoveRange(deletedRole);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserAdministrator(long userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                return user.UserRoles.Any(ur => ur.Role.Name.ToLower() == "administrator");
            }
            return false;
        }

        public async Task<UpdateRolesOfUserBaseViewModel> UpdateRolesOfUser(UpdateRolesOfUserCommandModel model)
        {
            var rolesToRemove = _context.UserRoles
                .Where(ur => ur.UserId == model.UserId && ur.RoleId != 3)
                .ToList();

            _context.UserRoles.RemoveRange(rolesToRemove);
            await _context.SaveChangesAsync();

            var roleToUserModel = new RoleToUserAddCommandModel()
            {
                UserId = model.UserId,
                RoleIds = model.RoleIds,
            };

            await AddRolesToUser(roleToUserModel);

            return new UpdateRolesOfUserBaseViewModel()
            {
                IsSucced = true,
            };

        }

        public async Task<bool> RolesExist(List<long> roleIds)
        {
            var matchingRoleCount = await _context.Roles
                .Where(role => roleIds.Contains(role.Id))
                .CountAsync();
            return matchingRoleCount == roleIds.Count;
        }

        public async Task<(int, IQueryable<Role>)> FilterAllAsync(RoleFilterCommandModel model)
        {
            var allRoles = _context.Roles
                .AsNoTracking()
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Where(r => !r.IsDeleted);

            if (model.RoleNames != null && model.RoleNames.Any())
            {
                allRoles = allRoles.Where(r => model.RoleNames.Contains(r.Name));
            }

            if (model.IsActive.HasValue)
            {
                allRoles = allRoles.Where(r => r.IsActive == model.IsActive);
            }

            return (await allRoles.CountAsync(), allRoles);
        }

        public async Task<GetRolesBaseViewModel> GethRolsWithPermissions(int page, int pageSize, IQueryable<Role> roles)
        {
            var pagination = await roles.OrderByDescending(a => a.CreatedAt).GetPaged(page, pageSize);

            var resultlist = pagination.Results.Select(role => new GetRolesWithPermissionPaginatedBaseViewModel()
            {
                RoleName = role.Name,
                PermissionsTitle = role.RolePermissions.Select(rp => rp.Permission.Title).ToList(),
                RoleId = role.Id,
                IsActive = role.IsActive,
            }).ToList();

            var rolesPermissions = new GetRolesBaseViewModel()
            {
                Elements = resultlist,
            };

            return rolesPermissions;
        }




        public async Task<List<Role>> GetRolesSearch(string? roleName)
        {
            var roles = _context.Roles
           .Where(r => r.IsDeleted == false);


            if (!string.IsNullOrEmpty(roleName))
            {
                roles = roles.Where(r => (r.Name ?? "").Contains(roleName));
                return await roles.OrderByDescending(a=>a.CreatedAt).ToListAsync();
            }
            else
            {
                return await roles.OrderByDescending(a => a.CreatedAt).ToListAsync();
            }
        }

        public async Task<Role> GetRoleWithPermissionsById(long RoleId)
        {
            return await _context.Roles.Include(a => a.RolePermissions).ThenInclude(a => a.Permission).Where(a => a.Id == RoleId).FirstOrDefaultAsync() ?? throw UserManagementExceptions.RoleNotFoundException; ;
        }

        public async Task ActiveInactiveRole(ActiveDeductiveRoleCommandModel model)
        {
            var role = await GetRoleById(model.RoleId);
            role.IsActive = model.IsActive;
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

        }
    }
}