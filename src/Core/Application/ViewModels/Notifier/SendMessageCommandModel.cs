using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Notifier
{
    public class SendMessageCommandModel
    {
        public int Channel { get; set; }
        public int MessageTypeId { get; set; }
        public string Destination { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
