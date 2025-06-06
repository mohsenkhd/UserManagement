using Application.ViewModels.Main;

namespace Application.ViewModels.AccountAggregate
{
    public class RegisterUserBaseViewMode : MainRes
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MobileNumber { get; set; }
        public bool? IsActive { get; set; }
        public long UserId { get; set; }

        public string? NationalCode { get; set; }

        public string? Email { get; set; }
        public long? AppId { get; set; }
    }
}