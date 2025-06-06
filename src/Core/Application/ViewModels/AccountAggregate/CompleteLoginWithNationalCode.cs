using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.AccountAggregate
{
    public class CompleteLoginWithNationalCode
    {
        public string OtpCode { get; set; } = null!;
        public string NationalCode { get; set; } = null!;
        [Required] public Guid CaptchaId { get; set; }
        [Required(ErrorMessage = "{0}نمیتواند خالی باشد")] public string CaptchaCode { get; set; } = null!;
    }
}