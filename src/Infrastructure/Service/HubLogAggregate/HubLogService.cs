using Application.RepositoryContracts.HubLogAggregate;
using Application.ServiceContracts.HubLogAggregate;
using Application.ViewModels.HubLog;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.HubLogAggregate
{
    public class HubLogService : IHubLogService
    {
        private readonly IHubLogRepository _hubLogRepository; 
        public HubLogService(IHubLogRepository hubLogRepository)
        {
                _hubLogRepository = hubLogRepository;
        }
        public async Task<AddHubLogViewModel> AddHubLog(AddHubLogCommandModel model)
        {
            var hubLogModel=new HubLog()
            {
                HubId=model.HubId,
                hubType=(HubType)model.hubType,
                UserId=model.UserId,
            };

          var addHubLog=  await _hubLogRepository.AddHub(hubLogModel);
            var res = new AddHubLogViewModel()
            {
                UserId = addHubLog.UserId,
                HubId = addHubLog.HubId,
                hubType =(HubTypeVm) addHubLog.hubType,
            };
            return res;
        }
        public async Task<GetHubLogsByUserIdViewModel> GetHubLogsByUserId(GetHubLogsByUserIdCommandModel model)
        {
          var hubLogs=  await _hubLogRepository.GetHubLogsByUserId(model.UserId);
            var hubLogsList = new List<GetHubLogsByUserId>();
           foreach (var hubLog in hubLogs)
            {

                hubLogsList.Add((GetHubLogsByUserId)hubLog);
            }
            return new GetHubLogsByUserIdViewModel()
            {
                getHubLogs = hubLogsList
            };
        }
        public AreAllHubIdsClosedOrOpenViewModel GetOpenHubTypeCountByHubId(AreAllHubIdsClosedOrOpenCommandModel model)
        {
            var GetOpenHubTypeCountByHubId = _hubLogRepository. GetOpenHubIdCountWithoutClose(model.userId);
            return new AreAllHubIdsClosedOrOpenViewModel()
            {
                CountOfOpenHub = GetOpenHubTypeCountByHubId
            };
        }

        public async Task RemoveHubLogsByUserId(RemoveHubLogsByUserIdCommandModel model)
        {
            await _hubLogRepository.RemoveHubLogsByUserId(model.UserId);
        }
    }
}
