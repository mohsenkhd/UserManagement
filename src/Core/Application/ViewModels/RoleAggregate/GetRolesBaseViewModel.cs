using Application.ViewModels.Main;
using Application.ViewModels.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RoleAggregate
{
    public class GetRolesBaseViewModel:MainRes
    {
        public int Page { get; set; }
        public bool HasNext { get; set; }


        public bool HasPrev { get; set; }


        public int TotalPages { get; set; }


        public long TotalElements { get; set; }

        public List<GetRolesWithPermissionPaginatedBaseViewModel> Elements { get; set; } = null!;
    }
}
