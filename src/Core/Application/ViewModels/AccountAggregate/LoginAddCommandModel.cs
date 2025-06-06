using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.AccountAggregate
{
    public class LoginAddCommandModel
    {
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        public string? MobileNumber { get; set; }

        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? NationalCode { get; set; }
        [Required] public Guid CaptchaId { get; set; }
        [Required(ErrorMessage = "{0}نمیتواند خالی باشد")] public string CaptchaCode { get; set; } = null!;
    }
}