namespace Domain.Entities
{
    public class Permission 

    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;

        public long? ParentId { get; set; }
        public bool? ForClient { get; set; }
        public Permission Parent { get; set; } = null!;
        public ICollection<Permission> Children { get; set; } = null!;
        public ICollection<RolePermission> RolePermissions { get; set; } = null!;
    }
}