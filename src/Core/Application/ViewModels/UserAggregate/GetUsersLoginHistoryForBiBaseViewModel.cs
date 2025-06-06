using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserAggregate
{
    public class GetUsersLoginHistoryForBiBaseViewModel
    {
        public long UserId { get; set; }
        public DateTime LogDate { get; set; }
        public UserHistoryType HistoryType { get; set; }
    }
}
