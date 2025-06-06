using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Captcha
{
    public class CheckCaptchaAddCommandModel
    {

        public CheckCaptchaAddCommandModel(Guid id, string code)
        {
            Id = id;
            Code = code;
        }

    
        public Guid Id { get; set; }

        
        public string Code { get; set; }
    }
}
