using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Context.DataSeeder
{
    public static class ApplicationSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context =
                new UserManagementContext(
                    serviceProvider.GetRequiredService<DbContextOptions<UserManagementContext>>());
            if (context.Applications.Any())
            {
                return;
            }

            var app = new List<Domain.Entities.Application>
            {
                new()
                {
                    Name = "IZB",
                    Password = "IZB!QAZ2wsx",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                },
            };

            context.Applications.AddRange(app);

            context.SaveChanges();
        }
    }
}