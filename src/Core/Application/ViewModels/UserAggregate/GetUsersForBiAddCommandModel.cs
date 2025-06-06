using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserAggregate
{
    public class GetUsersForBiAddCommandModel
    {
        public bool? IsActive { get; set; }
        public DateTime? FromDate{ get; set; }
        public DateTime? ToDate { get; set; }
    }
}
