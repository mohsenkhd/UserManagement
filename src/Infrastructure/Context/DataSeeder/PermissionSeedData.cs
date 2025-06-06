using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Context.DataSeeder
{
    public static class PermissionSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context =
                new UserManagementContext(
                    serviceProvider.GetRequiredService<DbContextOptions<UserManagementContext>>());
            var permissionsToAdd = new List<Permission>
            {
                new Permission { Id = 175, Title = "مدیریت کاربران", ParentId = null,ForClient = null },
                new Permission { Id = 176, Title = "احراز هویت", ParentId = 175,ForClient = null },
                new Permission { Id = 177, Title = "اضافه کردن دسترسی ها به نقش", ParentId = 176,ForClient = false },
                new Permission { Id = 178, Title = "دریافت دسترسی", ParentId = 176,ForClient = false },
                new Permission { Id = 179, Title = "به روز رسانی های دسترسی های نقش", ParentId = 176,ForClient = false },
                new Permission { Id = 180, Title = "اضافه کردن نقش با دسترسی", ParentId = 176,ForClient = false },
                new Permission { Id = 181, Title = "اضافه کردن نقش ها به کاربر", ParentId = 176,ForClient = false },
                new Permission { Id = 182, Title = "دریافت نقش ها با دسترسی ها", ParentId = 176,ForClient = false },
                new Permission { Id = 183, Title = "حذف نقش", ParentId = 176,ForClient = false },
                new Permission { Id = 184, Title = "به روزرسانی نقش ها", ParentId = 176,ForClient = false },
                new Permission { Id = 185, Title = "به روزرسانی نقش های کاربر", ParentId = 176,ForClient = false },
                new Permission { Id = 186, Title = "فعال و غیر فعال کردن نقش", ParentId = 176,ForClient = false },
                new Permission { Id = 187, Title = "جستجوی نقش ها", ParentId = 176,ForClient = false },
                new Permission { Id = 188, Title = "دریافت نقش", ParentId = 176,ForClient = false },
                new Permission { Id = 189, Title = "دریافت لیست کاربران", ParentId = 176,ForClient = false },
                new Permission { Id = 190, Title = "اضافه کردن کاربر", ParentId = 176,ForClient = false },
                new Permission { Id = 191, Title = "دربافت کاربر با نقش ها", ParentId = 176,ForClient = false },
                new Permission { Id = 192, Title = "به روزرسانی رمز عبور", ParentId = 176,ForClient = false },
                new Permission { Id = 193, Title = "به روزرسانی کاربران", ParentId = 176,ForClient = false },
                new Permission { Id = 194, Title = "تغییر وضعیت کاربر", ParentId = 176,ForClient = false },
                new Permission { Id = 195, Title = "دریافت کاربر با شناسه", ParentId = 176,ForClient = false },
                new Permission { Id = 196, Title = "اطلاعات کاربران", ParentId = 176,ForClient = false },
                new Permission { Id = 243, Title = "دریافت گزارش ثبت نام کاربران", ParentId = 176,ForClient = false },
                new Permission { Id = 340, Title = "دریافت کاربر با شماره مشتری", ParentId = 176,ForClient = false },
                new Permission { Id = 341, Title = "مشتریان", ParentId = null,ForClient = null },
                new Permission { Id = 342, Title = "نمایش اطلاعات مشتری", ParentId = 341,ForClient = true },
                new Permission { Id = 343, Title = "نمایش اطلاعات همه مشتریان", ParentId = 341,ForClient = false },
                new Permission { Id = 344, Title = "سفارشات", ParentId = null,ForClient = null },
                new Permission { Id = 345, Title = "نمایش سفارش مشتری", ParentId = 344,ForClient = true },
                new Permission { Id = 346, Title = "نمایش سفارش همه مشتریان", ParentId = 344,ForClient = false },
                new Permission { Id = 347, Title = "فاکتور ها", ParentId = null,ForClient = null },
                new Permission { Id = 348, Title = "نمایش فاکتور مشتری", ParentId = 347,ForClient = true },
                new Permission { Id = 349, Title = "نمایش فاکتور همه مشتریان", ParentId = 347,ForClient = false },
                new Permission { Id = 350, Title = "پرداخت فاکتور", ParentId = 347,ForClient = true },
                new Permission { Id = 351, Title = "ایجاد سفارش", ParentId = 344,ForClient = true },
                new Permission { Id = 352, Title = "کیف پول و تراکنش ها", ParentId = 175,ForClient = true },
            };
            var table = context.Permissions.ToList();
            if (table.Count == 0)
            {
                context.Permissions.AddRange(permissionsToAdd);
                context.SaveChanges();
            }

            if (permissionsToAdd.Count <= table.Count) return;
            var maxIdInTable = context.Permissions.Max(p => p.Id);
            var newPermissions = permissionsToAdd.Where(p => p.Id > maxIdInTable).ToList();

            if (newPermissions.Count <= 0) return;
            context.Permissions.AddRange(newPermissions);
            context.SaveChanges();
        }
    }
} 