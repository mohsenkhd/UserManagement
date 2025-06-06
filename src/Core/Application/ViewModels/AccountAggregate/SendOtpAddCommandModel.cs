using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountAggregate
{
    public class SendOtpAddCommandModel
    {
        public long UserId { get; set; }
        public TitleType Title { get; set; }
    }
    public enum TitleType
    {
        RegisterOtp = 1,
        LoginOtp = 2,
        ForgotOtp=3,

    }
}

