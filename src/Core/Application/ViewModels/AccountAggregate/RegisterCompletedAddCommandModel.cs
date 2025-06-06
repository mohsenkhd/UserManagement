using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.AccountAggregate
{
    public class RegisterCompletedAddCommandModel
    {
        public string OtpCode { get; set; } = null!;
        public long UserId { get; set; }
        [Required] public Guid CaptchaId { get; set; }
        [Required(ErrorMessage = "{0}نمیتواند خالی باشد")] public string CaptchaCode { get; set; } = null!;
    }
}