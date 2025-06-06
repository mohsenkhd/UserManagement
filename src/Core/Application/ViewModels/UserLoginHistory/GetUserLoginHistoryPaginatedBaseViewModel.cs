using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserLoginHistory
{
    public class GetUserLoginHistoryPaginatedBaseViewModel
    {
        public long Id { get; set; }
        public long UserFk { get; set; }
        public DateTime LogDate { get; set; }
        public UserHistoryType HistoryType { get; set; }
        public long? CustomerNumber { get; set; }
    }
}
