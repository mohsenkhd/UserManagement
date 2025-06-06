using Common.Helper;
using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Context.DataSeeder
{
    public static class WalletSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new UserManagementContext(serviceProvider.GetRequiredService<DbContextOptions<UserManagementContext>>());
            if (context.Wallets.Any())
            {
                return;
            }
            
            var app = new List<Wallet>
            {
                new Wallet
                {
                    UserId = 2 ,
                    Balance = 1500000,
                    CreatedAt = DateTime.Now,
                    UpdatedAt=DateTime.Now,
                    IsDeleted = false
                }
            };

            context.Wallets.AddRange(app);
            
            context.SaveChanges();
        }
    }
}
