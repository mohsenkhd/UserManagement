using Application.ViewModels.UserAggregate;
using Application.ViewModels.UserLoginHistory;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceContracts.UsersLoginHistory
{
    public interface IUsersLoginHistoryService
    {
        Task<UserLoginHistoryBaseViewModel> AddUserLoginHistory(UserLoginHistoryAddCommandModel model);
        Task<UserLoginHistoryBaseViewModel> GetUserLastStatus(long userId);
        Task<RemoveUserLoginHistoryViewModel> RemoveUserLoginHistory(RemoveUserLoginHistoryCommandModel model);
        Task<GetUserLoginHistoryBaseViewModel> GetUserLoginHistory(GetUserLoginHistoryAddCommandModel req);
 
    }
}
