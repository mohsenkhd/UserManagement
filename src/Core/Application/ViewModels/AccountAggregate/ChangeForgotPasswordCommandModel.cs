using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.ViewModels.AccountAggregate
{
    public class ChangeForgotPasswordCommandModel
    {
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = null!;
        [DataType(DataType.Password)]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string? ConfirmPassword { get; set; } = null!;
        [Required]
        public string Code { get; set; } = null!;
    }
}
