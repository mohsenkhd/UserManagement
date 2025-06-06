using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountAggregate
{
    public class FullTokenResponseCommandModel
    {
        public List<long> PermissionIds { get; set; } = null!;
        public long UserId { get; set; }
        public long ApplicationId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Phone { get; set; } = null!;
        public bool IsAdmin { get; set; } = false;
        public long? CustomerNumber { get; set; }
    }
}
