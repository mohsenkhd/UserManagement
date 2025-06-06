using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.AccountAggregate
{
    public class LoginWithNationalCodeAddCommandModel
    {
        [Required] public string NationalCode { get; set; } = null!;
        [Required] public Guid CaptchaId { get; set; }
        [Required(ErrorMessage = "{0}نمیتواند خالی باشد")] public string CaptchaCode { get; set; } = null!;
    }
}