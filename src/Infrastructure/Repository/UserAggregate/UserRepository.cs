using Application.RepositoryContracts.UserAggregate;
using Domain.Entities;
using Context.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.ViewModels.RoleAggregate;
using Application.ViewModels.AccountAggregate;
using Common.Exceptions.UserManagement;
using Common.Pagination;
using Application.ViewModels.UserAggregate;

namespace Repository.UserAggregate
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManagementContext _context;
        private readonly IConfiguration _configuration;
        IQueryable<User> AllUsers;

        public UserRepository(UserManagementContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            AllUsers = dbContext.Users;
            _configuration = configuration;
        }

        public async Task<User> RegisterUser(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetById(long userId)
        {
            var res = await _context.Users.FindAsync(userId);
            return res ?? throw UserManagementExceptions.UserNotFoundException;
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }


        public async Task<User?> GetUserByMobileNumber(string mobileNumber)
        {
            return await _context.Users.Where(a => a.Phone == mobileNumber && a.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByNationalCode(string nationalCode)
        {
            return await _context.Users.Where(a => a.NationalCode == nationalCode && a.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetUserRoleNames(long userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new List<string>();
            }

            var roleNames = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            return roleNames;
        }

        public async Task<bool> CheckUserPermissionAsync(GetRolesAndUserIdFromTokenAsyncAddCommandModel model)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Id == model.userId);

            if (user == null)
            {
                return false; // کاربر پیدا نشد.
            }

            foreach (var role in user.UserRoles.Select(ur => ur.Role))
            {
                var permission = role.RolePermissions
                    .Select(rp => rp.Permission)
                    .FirstOrDefault(p => p.Title == model.title);
                if (permission != null)
                {
                    return true; // دسترسی پیدا شد.
                }
            }

            return false; // دسترسی پیدا نشد.
        }

        public async Task<List<long>> GetUserPermissionIds(GetUserPermissionIdsCommandModel model)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Id == model.UserId);

            if (user == null)
            {
                return new List<long>();
            }

            var permissionIds = user.UserRoles
                .Where(ur => ur.Role?.IsActive == true)
                .SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Id))
                .Distinct()
                .ToList();

            return permissionIds;
        }
        public async Task<bool> CheckUserIsAdmin(long userId)
        {
            var isUserAdmin = await _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.UserRoles.Select(ur => ur.Role.RolePermissions
                    .Select(rp => rp.Permission.ForClient)))
                .ToListAsync();

            var anyFalseForClient = isUserAdmin.SelectMany(x => x).Any(forClient => forClient == false);

            return anyFalseForClient;
        }


        public async Task<List<User>> GetByIds(List<long> ids)
        {
            return await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
        }

        public async Task<GetUsersWithRolesBaseViewModel> GetUsersWithRoles(int page, int pageSize)
        {
            var pagination = await AllUsers.GetPaged(page, pageSize);

            var resultlist = new List<GetUsersWithRolesPaginatedBaseViewModel>();

            foreach (var item in pagination.Results)
            {
                var viewModel = await GetUserWithRols(new GetUserWithRoleCommandModel { UserId = item.Id });

                resultlist.Add((GetUsersWithRolesPaginatedBaseViewModel)viewModel);
            }

            var userRole = new GetUsersWithRolesBaseViewModel()
            {
                Elements = resultlist,
            };

            return userRole;
        }

        public async Task<int> FilterAllAsync(UserFilterCommandModel model)
        {
            AllUsers = _context.Users
               .Include(user => user.UserRoles)
               .ThenInclude(userRole => userRole.Role)
               .Where(user => !user.IsDeleted).OrderByDescending(a => a.Id);

            if (!string.IsNullOrEmpty(model.FirstName))
            {
                AllUsers = AllUsers.Where(user => ((user.FirstName ?? "").ToLower()).Contains(model.FirstName.ToLower()));
            }

            if (!string.IsNullOrEmpty(model.LastName))
            {
                AllUsers = AllUsers.Where(user => ((user.LastName ?? "").ToLower()).Contains(model.LastName.ToLower()));
            }

            if (!string.IsNullOrEmpty(model.MobileNumber))
            {
                AllUsers = AllUsers.Where(user => (user.Phone ?? "").Contains(model.MobileNumber));
            }

            if (!string.IsNullOrEmpty(model.NationalCode))
            {
                AllUsers = AllUsers.Where(user => (user.NationalCode ?? "").Contains(model.NationalCode));
            }

            if (model.IsActive.HasValue)
            {
                AllUsers = AllUsers.Where(user => user.IsActive == model.IsActive);
            }

            if (model.RoleNames != null && model.RoleNames.Any())
            {
                var roleNames = model.RoleNames;
                AllUsers = AllUsers.Where(user => user.UserRoles.Any(userRole => roleNames.Contains(userRole.Role.Name)));
            }

            return await AllUsers.CountAsync();
        }
        public async Task<GetUserWithRoleBaseViewModel> GetUserWithRols(GetUserWithRoleCommandModel req)
        {
            var userWithRoles = await _context.Users
                .Where(a => a.Id == req.UserId)
                .Select(a => new GetUserWithRoleBaseViewModel
                {
                    UserId = a.Id,
                    Email = a.Email,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    IsActive = a.IsActive,
                    MobileNumber = a.Phone,
                    NationalCode = a.NationalCode,
                    CreateDate = a.CreatedAt,
                    UserRoles = a.UserRoles.Select(r => new RolesOfUserBaseViewModel
                    {
                        RoleDescription = r.Role.Description,
                        RoleName = r.Role.Name,
                        RoleId = r.Role.Id
                    }).ToList()
                })
                .FirstAsync();
            return userWithRoles;
        }
        public async Task<List<UserLoginHistory>> GetUsersLoginHistoryForBi(GetUsersLoginHistoryForBiAddCommandModel model)
        {
            var query = _context.UserLoginHistories.AsQueryable();



            if (model.FromDate.HasValue)
            {
                query = query.Where(user => user.LogDate >= model.FromDate.Value);
            }

            if (model.ToDate.HasValue)
            {
                query = query.Where(user => user.LogDate <= model.ToDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<User>> FilterForBiAsync(GetUsersForBiAddCommandModel model)
        {
            var query = _context.Users.Where(user => !user.IsDeleted && user.RegisterComplete);

            if (model.IsActive.HasValue)
            {
                query = query.Where(user => user.IsActive == model.IsActive.Value);
            }

            if (model.FromDate.HasValue)
            {
                query = query.Where(user => user.CreatedAt >= model.FromDate.Value);
            }

            if (model.ToDate.HasValue)
            {
                query = query.Where(user => user.CreatedAt <= model.ToDate.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<int> GetTotalUserCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetUsersCreatedLastMonthAsync()
        {
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            return await _context.Users.Where(u => u.CreatedAt >= oneMonthAgo).CountAsync();
        }

        public async Task<int> GetUsersCreatedLastWeekAsync()
        {
            DateTime oneWeekAgo = DateTime.Now.AddDays(-7);
            return await _context.Users.Where(u => u.CreatedAt >= oneWeekAgo).CountAsync();
        }

        public async Task<int> GetUsersCreatedYesterdayAsync()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            return await _context.Users.Where(u => u.CreatedAt >= yesterday).CountAsync();
        }

        public async Task<List<long>> GetUsersWithRoleId(long roleId)
        {
            var users = await _context.Roles
                .Where(r => r.Id == roleId)
                .SelectMany(r => r.UserRoles)
                .Include(ur => ur.User)
                .Select(ur => ur.User.Id)
                .ToListAsync();

            return users;
        }

        public async Task<User> GetByCustomerNumber(long customerNumber)
        {
            var res = await _context.Users.FirstOrDefaultAsync(x => x.CustomerNumber == customerNumber);
            return res ?? throw UserManagementExceptions.UserNotFoundException;
        }
    }
}