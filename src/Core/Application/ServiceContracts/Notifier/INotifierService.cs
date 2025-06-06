using Application.ViewModels.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceContracts.Notifier
{
    public interface INotifierService
    {
        Task<SendMessageBaseViewModel> SendMessageAsync(SendMessageCommandModel model);
    }
}
