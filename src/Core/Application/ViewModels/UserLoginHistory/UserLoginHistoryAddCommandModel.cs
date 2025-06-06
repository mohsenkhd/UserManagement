using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserLoginHistory
{
    public class UserLoginHistoryAddCommandModel
    {
        public long UserId { get; set; }
        public DateTime LogDate { get; set; }
        public long? CostumerNumber { get; set; }
        public UserHistoryTypeVm HistoryTypeVm { get; set; }
    }
    public enum UserHistoryTypeVm
    {
        Login = 1,
        Logout = 2
    }
}
