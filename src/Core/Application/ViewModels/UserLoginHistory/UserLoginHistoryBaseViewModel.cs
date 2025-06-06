using Application.ViewModels.Main;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserLoginHistory
{
    public class UserLoginHistoryBaseViewModel : MainRes
    {
        public long UserId { get; set; }
        public DateTime LogDate { get; set; }
        public long? CostumerNumber { get; set; }
        public UserHistoryType HistoryType { get; set; }
    }
}
