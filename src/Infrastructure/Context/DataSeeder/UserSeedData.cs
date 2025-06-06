using Common.Helper;
using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Context.DataSeeder
{
    public static class UserSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new UserManagementContext(serviceProvider.GetRequiredService<DbContextOptions<UserManagementContext>>());
            if (context.Users.Any())
            {
                return;
            }
            var passwordHelper = new PasswordHelper();
            var app = new List<User>
            {
                new User
                {
                    FirstName = "Mohsen",
                    LastName ="Khodaparast",
                    Phone = "09391615711",
                    ApplicationFk=1,
                    IsActive=true,
                    NationalCode = "1129936171",
                    Password=passwordHelper.EncodePasswordBcrypt("zse098$"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt=DateTime.Now,
                    IsDeleted = false
                }, new User
                {
                    FirstName = "Arash",
                    LastName ="Ahmadi",
                    Phone = "09333568525",
                    NationalCode = "1129936121",
                    ApplicationFk=1,
                    IsActive=true,
                    Password=passwordHelper.EncodePasswordBcrypt("arash12345#"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt=DateTime.Now,
                    IsDeleted = false
                },
            };

            context.Users.AddRange(app);

            context.SaveChanges();
        }
    }
}
