using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RoleAggregate
{
    public class GetRolesWithPermissionPaginatedBaseViewModel
    {
        public  string RoleName { get; set; } = null!;
        public List< string> PermissionsTitle { get; set; }=null!;
        public long RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}
