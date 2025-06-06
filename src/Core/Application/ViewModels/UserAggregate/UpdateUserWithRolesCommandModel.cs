using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.ViewModels.UserAggregate
{
    public class UpdateUserWithRolesCommandModel
    {
        public long UserId { get; set; }
 
        public string? FirstName { get; set; } = null!;

        public string? LastName { get; set; } = null!;
        public string? NationalCode { get; set; }
        public string MobileNumber { get; set; } = null!;

        public List<long> RoleIds { get; set; } = null!;
    }
}
