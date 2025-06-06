using Application.RepositoryContracts.HubLogAggregate;
using Application.RepositoryContracts.UsersLoginHistoryAggregate;
using Application.ServiceContracts.UsersLoginHistory;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.UserAggregate;
using Application.ViewModels.UserLoginHistory;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UsersLoginHistoryAggregate
{
    public class UsersLoginHistoryService : IUsersLoginHistoryService
    {
        private readonly IUsersLoginHistoryRepository _usersLoginHistoryRepository;
        private readonly IHubLogRepository _hubLogRepository;
        public UsersLoginHistoryService(IUsersLoginHistoryRepository usersLoginHistoryRepository, IHubLogRepository hubLogRepository)
        {
            _usersLoginHistoryRepository = usersLoginHistoryRepository;
            _hubLogRepository = hubLogRepository;
        }
        public async Task<UserLoginHistoryBaseViewModel> AddUserLoginHistory(UserLoginHistoryAddCommandModel model)
        {

            var userHistoryModel = new UserLoginHistory()
            {
                UserFk = model.UserId,
                HistoryType = (UserHistoryType)model.HistoryTypeVm,
                LogDate = model.LogDate,
                CustomerNumber = model.CostumerNumber.GetValueOrDefault()
            };
            var res = await _usersLoginHistoryRepository.AddUserLoginHistory(userHistoryModel);
            var resModel = new UserLoginHistoryBaseViewModel()
            {
                LogDate = res.LogDate,
                HistoryType = res.HistoryType,
                UserId = res.UserFk,
                CostumerNumber = res?.CustomerNumber

            };

            return resModel;
        }

        public async Task< UserLoginHistoryBaseViewModel> GetUserLastStatus(long userId)
        {
            var userLoginHistorieLastStatus = await _usersLoginHistoryRepository.GetUserLastStatuses(userId);
            if (userLoginHistorieLastStatus == null)
            {
                return new UserLoginHistoryBaseViewModel()
                {
                    CostumerNumber = null,
                    HistoryType = UserHistoryType.Logout,
                    LogDate = DateTime.Now,
                    UserId = 0
                };
            }
            else
            {
                var res = new UserLoginHistoryBaseViewModel()
                {
                    CostumerNumber = userLoginHistorieLastStatus.CustomerNumber??0,
                    HistoryType = userLoginHistorieLastStatus.HistoryType,
                    LogDate = userLoginHistorieLastStatus.LogDate,
                    UserId = userLoginHistorieLastStatus.UserFk
                };


                return res;
            }
        
        }

        public async Task<RemoveUserLoginHistoryViewModel> RemoveUserLoginHistory(RemoveUserLoginHistoryCommandModel model)
        {
            
            var hubStatuses=await _hubLogRepository.GetHubLastStatuses(model.UserId);
            var hubType= hubStatuses.Select(a=>a.hubType).ToList();

            if (hubType[0]==HubType.Open && hubType[1] == HubType.Close) {

                
     

               await _usersLoginHistoryRepository.RemoveUserLoginHistory(model.UserId);
                return new RemoveUserLoginHistoryViewModel()
                {
                    IsSuccess = true,
                };
            }
            return new RemoveUserLoginHistoryViewModel()
            {
                IsSuccess = false,
            };

        }
        public async Task<GetUserLoginHistoryBaseViewModel> GetUserLoginHistory(GetUserLoginHistoryAddCommandModel req)
        {
            var filterAllModel = new UserLoginHistoryFilterCommandModel()
            {
                UserId=req.UserId,
            };
            var count = await _usersLoginHistoryRepository.FilterAllAsync(filterAllModel);
            var userLogs = await _usersLoginHistoryRepository.GetUserLoginHistory(req.Page, req.PerPage);
            var res = new GetUserLoginHistoryBaseViewModel()
            {
                Elements = userLogs.Elements,
                TotalElements = count,
                TotalPages = (int)Math.Ceiling(count / (decimal)req.PerPage),
                HasNext = (req.Page + 1) < (int)Math.Ceiling(count / (decimal)req.PerPage),
                HasPrev = req.Page > 0 && (int)Math.Ceiling(count / (decimal)req.PerPage) != 0,
                Page = req.Page
            };
            return res;
        }
    }
}
