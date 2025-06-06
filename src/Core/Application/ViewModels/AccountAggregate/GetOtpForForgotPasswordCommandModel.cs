using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountAggregate
{
    public class GetOtpForForgotPasswordCommandModel
    {
        public long UserId { get; set; }
        public string Otp { get; set; } = null!;
        [Required] public Guid CaptchaId { get; set; }
        [Required(ErrorMessage = "{0}نمیتواند خالی باشد")] public string CaptchaCode { get; set; } = null!;
    }
}
