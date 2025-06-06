using Application.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserAggregate
{
    public class UserCountReportViewModel:MainRes
    {
        public int AllUsersCount { get; set; }
        public int LastMonthUsersCount { get; set; }
        public int LastWeekUsersCount { get; set; }
        public int YesterdayUsersCount { get; set; }
    }
}
