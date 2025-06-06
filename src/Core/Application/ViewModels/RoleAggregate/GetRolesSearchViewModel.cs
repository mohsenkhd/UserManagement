using Application.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RoleAggregate
{
    public class GetRolesSearchViewModel:MainRes
    {
        public List<GetRolesSearch> Roles { get; set; } = new List<GetRolesSearch>();
    }
}
