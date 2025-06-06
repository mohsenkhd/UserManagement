using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.AccountAggregate
{
    public class RegisterUserAddCommandModel
    {
        public long? UserId { get; set; }
        [Display(Name = "نام ")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string? FirstName { get; set; } = null!;
        [Display(Name = "نام خانوادگی ")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string? LastName { get; set; } = null!;

        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        public string MobileNumber { get; set; } = null!;
        [Display(Name = "کلمه عبور")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = null!;
        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string? ConfirmPassword { get; set; } = null!;
        public string? NationalCode { get; set; }
        [Required] public Guid CaptchaId { get; set; }
        [Required(ErrorMessage = "{0}نمیتواند خالی باشد")] public string CaptchaCode { get; set; } = null!;
        public long AppId { get; set; }
        public RegisterType RegisterType { get; set; }
    }
    public enum RegisterType
    {
        RegisterWithPassword = 1,
        RegisterWithOtp = 2,
        RegisterWithPasswordAndOtp = 3,
    }
}