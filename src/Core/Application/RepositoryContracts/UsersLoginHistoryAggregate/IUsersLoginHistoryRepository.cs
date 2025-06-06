using Application.ViewModels.UserAggregate;
using Application.ViewModels.UserLoginHistory;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryContracts.UsersLoginHistoryAggregate
{
    public interface IUsersLoginHistoryRepository
    {
        Task<UserLoginHistory> AddUserLoginHistory(UserLoginHistory model);
        Task<UserLoginHistory> GetUserLastStatuses(long userId);
        Task RemoveUserLoginHistory(long userId);
        Task<int> FilterAllAsync(UserLoginHistoryFilterCommandModel model);
        Task<GetUserLoginHistoryBaseViewModel> GetUserLoginHistory(int page, int pageSize);
    }
}
