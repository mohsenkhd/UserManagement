using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryContracts.HubLogAggregate
{
    public interface IHubLogRepository
    {
        Task<HubLog>AddHub(HubLog model);
        Task<List<HubLog>> GetHubLogsByUserId(long userId);
        Task<List<HubLog>> GetHubLastStatuses(long userId);
        int GetOpenHubIdCountWithoutClose(long userId);
        Task RemoveHubLogsByUserId(long userId);
    }
}
