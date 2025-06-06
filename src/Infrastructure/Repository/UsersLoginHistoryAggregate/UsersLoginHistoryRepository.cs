using Application.RepositoryContracts.UsersLoginHistoryAggregate;
using Application.ViewModels.UserAggregate;
using Application.ViewModels.UserLoginHistory;
using Common.Pagination;
using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.UsersLoginHistoryAggregate
{
    public class UsersLoginHistoryRepository : IUsersLoginHistoryRepository
    {
        private readonly UserManagementContext _context;
        IQueryable<UserLoginHistory> AllUserlogs;
        public UsersLoginHistoryRepository(UserManagementContext dbContext)
        {
            _context = dbContext;
            AllUserlogs = dbContext.UserLoginHistories;
        }
        public async Task<UserLoginHistory> AddUserLoginHistory(UserLoginHistory model)
        {
            await _context.UserLoginHistories.AddAsync(model);

            await _context.SaveChangesAsync();

            return model;
        }

      
        public async Task< UserLoginHistory> GetUserLastStatuses(long userId)
        {
            var res = await _context.UserLoginHistories.Where(a => a.UserFk == userId).OrderByDescending(u => u.LogDate).AsNoTracking().FirstOrDefaultAsync();
            return res ;
        }
        public async Task RemoveUserLoginHistory(long userId)
        {
        
                var historyList = await _context.UserLoginHistories.Where(a => a.UserFk == userId && a.HistoryType== UserHistoryType.Logout).ToListAsync();
                foreach (var item in historyList)
                {
                    if (_context.Entry(item).State == EntityState.Detached)
                    {
                        _context.UserLoginHistories.Attach(item);
                    }
                    _context.UserLoginHistories.Remove(item);
                }
                await _context.SaveChangesAsync();
        }
        public async Task<int> FilterAllAsync(UserLoginHistoryFilterCommandModel model)
        {
            AllUserlogs = _context.UserLoginHistories
          .Where(u => u.UserFk==model.UserId);

            return await AllUserlogs.CountAsync();
        }
        public async Task<GetUserLoginHistoryBaseViewModel> GetUserLoginHistory(int page, int pageSize)
        {
            
            var pagination = await  AllUserlogs.OrderByDescending(a=>a.LogDate).GetPaged(page, pageSize);
            var resultlist = new List<GetUserLoginHistoryPaginatedBaseViewModel>();

            foreach (var item in pagination.Results)
            {
                resultlist.Add(new GetUserLoginHistoryPaginatedBaseViewModel()
                {
                    Id = item.Id,
                    CustomerNumber = item.CustomerNumber,
                    HistoryType = item.HistoryType,
                    LogDate = item.LogDate,
                    UserFk = item.UserFk
                });
            }

            var userLog = new GetUserLoginHistoryBaseViewModel()
            {
                Elements = resultlist,
            };

            return userLog;
        }

    }
}
