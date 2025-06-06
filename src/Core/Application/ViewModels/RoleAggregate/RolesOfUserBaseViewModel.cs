namespace Application.ViewModels.RoleAggregate
{
    public class RolesOfUserBaseViewModel
    {
        public string RoleName { get; set; } = null!;
        public string? RoleDescription { get; set; }
        public long RoleId { get; set; }
    }
}