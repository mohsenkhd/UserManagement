using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Notifier
{
    public class SendMessageBaseViewModel
    {
        public long Code { get; set; }
        public long MessageId { get; set; }
        public bool Sent { get; set; }
        public long ReferenceId { get; set; }
        public string ClientMessage { get; set; } = null!;
    }
}
