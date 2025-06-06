using Application.ViewModels.HubLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceContracts.HubLogAggregate
{
    public interface IHubLogService
    {
        Task<AddHubLogViewModel> AddHubLog(AddHubLogCommandModel model);
        Task<GetHubLogsByUserIdViewModel> GetHubLogsByUserId(GetHubLogsByUserIdCommandModel model);
        AreAllHubIdsClosedOrOpenViewModel GetOpenHubTypeCountByHubId(AreAllHubIdsClosedOrOpenCommandModel model);
        Task RemoveHubLogsByUserId(RemoveHubLogsByUserIdCommandModel model);
    }
}
