using Application.ViewModels.Main;

namespace Application.ViewModels.AccountAggregate
{
    public class GetOtpForForgotPasswordViewModel : MainRes
    {
        public string Token { get; set; } = null!;
    }
}
