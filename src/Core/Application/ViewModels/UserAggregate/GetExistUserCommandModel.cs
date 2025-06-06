namespace Application.ViewModels.UserAggregate
{
    public class GetExistUserCommandModel
    {
        public long? UserId { get; set; }

        public string MobileNumber { get; set; } = null!;

        public string? NationalCode { get; set; }
    }
}