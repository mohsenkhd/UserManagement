namespace Domain.Entities
{
    public class Role : Domain.Base.Base
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }=null!;
        public ICollection<RolePermission> RolePermissions { get; set; } = null!;
    }
}