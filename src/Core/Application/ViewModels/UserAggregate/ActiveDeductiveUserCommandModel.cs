using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserAggregate
{
    public class ActiveDeductiveUserCommandModel
    {
        public long UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
