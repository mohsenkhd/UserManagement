using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ClientAggregate
{
    public class GetClientViewModel
    {
        public string Name { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public string ApiSecret { get; set; } = null!;
        public string[]? AuthorizedIps { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
