using Application.ViewModels.Fundamental;
using Application.ViewModels.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceContracts.Fundamental
{
    public interface IFundamentalService
    {
        Task<ShahkarBaseViewModel> Shahkar(ShahkarCommandModel model);
        Task<CustomerBaseViewModel> Customer(CustomerCommandModel model);
        Task<ContentExceptionBaseViewModel> ContentException(ContentExceptionCommandModel model);
    }
}
