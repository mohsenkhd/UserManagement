using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Context.DataSeeder
{
    public static class RolePermissionSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context =
                new UserManagementContext(
                    serviceProvider.GetRequiredService<DbContextOptions<UserManagementContext>>());
            CompareRolePermissions(context, 1, false, false);
            CompareRolePermissions(context, 2, false, true);
        }

        private static void CompareRolePermissions(UserManagementContext context, int roleId, bool getAll, bool forClient)
        {
            var rolePermIds = context.RolePermissions.Where(a => a.RoleId == roleId).ToList();
            var permissionsQ = context.Permissions.AsQueryable();
            if (!getAll)
            {
                permissionsQ = permissionsQ.Where(p => p.ForClient == forClient);
            }
            var permissions = permissionsQ.ToList();
            if (permissions.Count <= rolePermIds.Count) return;
            context.RolePermissions.RemoveRange(rolePermIds);
            foreach (var perm in permissions)
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = perm.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                });
            }
            context.SaveChanges();
        }

    }
}