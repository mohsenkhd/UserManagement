using Application.ViewModels.ClientAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceContracts.ClientAggregate
{
    public interface IClientService
    {
        Task<GetClientViewModel> GetClient(GetClientCommandModel model);
    }
}
