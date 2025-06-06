using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Context.DataSeeder
{
    public static class RoleSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context =
                   new UserManagementContext(
                       serviceProvider.GetRequiredService<DbContextOptions<UserManagementContext>>()))
            {
                var role = new List<Role>
                {
                    new Role
                    {
                        Name = "Admin",
                        Description = "Admin is a role that has admin permissions ",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                    },
                    new Role
                    {
                        Name = "Customer",
                        Description = "a Role for customer",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    }
                };

                var roleName = role.Select(x => x.Name).ToList();
                var rolesExist = context.Roles
                    .Where(r => roleName.Contains(r.Name))
                    .ToList();

                if (rolesExist.Count == 0)
                {
                    context.Roles.AddRange(role);
                    context.SaveChanges();
                }
            }
        }
    }
}