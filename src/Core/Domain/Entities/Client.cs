using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Client : Domain.Base.Base
    {
        public string Name { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public string ApiSecret { get; set; } = null!;
        public string? AuthorizedIps { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
