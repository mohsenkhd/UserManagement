using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Captcha
{
    public class CheckCaptchaBaseViewModel
    {
        public CheckCaptchaBaseViewModel(bool status)
        {
            Status = status;
        }

        public bool Status { get; set; }
    }
}
