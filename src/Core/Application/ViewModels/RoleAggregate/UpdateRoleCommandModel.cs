using Application.ViewModels.Main;

namespace Application.ViewModels.RoleAggregate
{
    public class UpdateRoleCommandModel
    {
        public long RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<long> PermissionsIds { get; set; } = null!;
    }
}
