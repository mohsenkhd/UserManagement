using Application.ViewModels.RoleAggregate;
using System.ComponentModel;

namespace Application.ViewModels.UserAggregate
{
    public class GetUsersWithRolesAddCommandModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public List<string>? RoleNames { get; set; }
        public string? NationalCode { get; set; }
        public bool? IsActive { get; set; }
        [DefaultValue(1)] public int Page { get; set; }
        [DefaultValue(4)] public int PerPage { get; set; }
    }
}