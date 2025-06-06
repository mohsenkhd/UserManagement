using Application.ViewModels.Main;

namespace Application.ViewModels.RoleAggregate
{
    public class UpdateRoleBaseViewModel:MainRes
    {
        public long RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
