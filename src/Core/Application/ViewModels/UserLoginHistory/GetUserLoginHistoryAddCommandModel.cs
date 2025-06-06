using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserLoginHistory
{
    public class GetUserLoginHistoryAddCommandModel
    {
        public long UserId { get; set; }
        [DefaultValue(1)] public int Page { get; set; }
        [DefaultValue(4)] public int PerPage { get; set; }
    }
}
