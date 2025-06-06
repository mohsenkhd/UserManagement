using Application.ViewModels.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserAggregate
{
    public class UserFilterCommandModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public List<string>? RoleNames { get; set; }
        public string? NationalCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
