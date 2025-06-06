using Application.ViewModels.Main;
using Application.ViewModels.PermissionAggregate;


namespace Application.ViewModels.RoleAggregate
{
    public class GetRoleWithPermissionsByIdBaseViewModel:MainRes
    {
        public string RoleName { get; set; } = null!;
        public string? RoleDescription { get; set; }
        public bool? IsActive { get; set; }
        public List<PermissionResultViewModel> Permissions { get; set; }= new List<PermissionResultViewModel>();
    }
}
