using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Context.DataSeeder
{
    public static class UserRoleSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context =
                   new UserManagementContext(
                       serviceProvider.GetRequiredService<DbContextOptions<UserManagementContext>>()))
            {
                if (context.UserRoles.Any())
                {
                    return;
                }

                var userRole = new List<UserRole>
                {
                    new UserRole
                    {
                        UserId = 1,
                        RoleId = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false
                    },
                    new UserRole
                    {
                        UserId = 2,
                        RoleId = 2,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false
                    }
                };

                context.UserRoles.AddRange(userRole);

                context.SaveChanges();
            }
        }
    }
}