using Application.ViewModels.Main;

namespace Application.ViewModels.AccountAggregate
{
    public class LoginWithNationalCodeBaseViewModel : MainRes
    {
        public bool IsSend { get; set; }
        public string MobileNumber { get; set; } = null!;
        public long UserId { get; set; }
    }
}