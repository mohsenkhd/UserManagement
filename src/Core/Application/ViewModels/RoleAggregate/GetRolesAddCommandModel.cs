using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RoleAggregate
{
    public class GetRolesAddCommandModel
    {
        public List< string>? Names { get; set; }
        public bool? IsActive { get; set; }
        [DefaultValue(1)] public int Page { get; set; }
        [DefaultValue(4)] public int PerPage { get; set; }
    }
}
