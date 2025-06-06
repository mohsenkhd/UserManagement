using Application.ViewModels.Main;
using Application.ViewModels.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserLoginHistory
{
    public class GetUserLoginHistoryBaseViewModel:MainRes
    {
        public int Page { get; set; }
        public bool HasNext { get; set; }


        public bool HasPrev { get; set; }


        public int TotalPages { get; set; }


        public long TotalElements { get; set; }

        public List<GetUserLoginHistoryPaginatedBaseViewModel> Elements { get; set; } = null!;
    }
}
