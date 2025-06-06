using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RoleAggregate
{
    public class ActiveDeductiveRoleCommandModel
    {
        public long RoleId{ get; set; }
        public bool IsActive { get; set; }
    }
}
