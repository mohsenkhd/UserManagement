using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountAggregate
{
    public class GetUsersInfoAddCommandModel
    {
        public List<long> UsersIds { get; set; } = new List<long>();

    }
}
