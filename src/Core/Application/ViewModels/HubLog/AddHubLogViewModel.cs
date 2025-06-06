using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.HubLog
{
    public class AddHubLogViewModel
    {
        public string HubId { get; set; } = null!;
        public long UserId { get; set; }
        public HubTypeVm hubType { get; set; }
    }
}

