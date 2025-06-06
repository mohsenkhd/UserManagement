namespace Application.ViewModels.Signalr
{
    public class UserInfo
    {
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public long? CostumerNumber { get; set; }
        public DateTime RecordDate { get; set; }
    }
}
