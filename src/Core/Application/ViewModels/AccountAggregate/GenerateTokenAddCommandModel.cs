namespace Application.ViewModels.AccountAggregate
{
    public class GenerateTokenAddCommandModel
    {
        public List<long> PermissionIds { get; set; } = null!;
        public long UserId { get; set; }
        public long ApplicationId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Phone { get; set; } = null!;
        public bool IsAdmin { get; set; } = false;
        public long? CustomerNumber { get; set; }
    }
}