namespace Application.ViewModels.RoleAggregate
{
    public class RoleToUserAddCommandModel
    {
        public List<long> RoleIds { get; set; } = null!;
        public long UserId { get; set; }
    }
}