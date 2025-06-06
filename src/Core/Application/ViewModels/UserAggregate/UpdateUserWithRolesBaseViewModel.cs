using Application.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserAggregate
{
    public class UpdateUserWithRolesBaseViewModel:MainRes
    {
        public long UserId { get; set; }
    }
}
