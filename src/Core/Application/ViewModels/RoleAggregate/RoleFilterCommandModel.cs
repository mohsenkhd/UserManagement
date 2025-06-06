using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RoleAggregate
{
    public class RoleFilterCommandModel
    {
        public List< string>? RoleNames { get; set; }
        public bool? IsActive { get; set; }

    }
}
