using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.PermissionAggregate
{
    public class PermissionResultViewModel
    {
        public long PermissionId { get; set; }
        public string Title { get; set; } = null!;

        public long? ParentId { get; set; }
    }
}
