using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountAggregate
{
    public class GetRolesAndUserIdFromTokenAsyncAddCommandModel
    {
        public long userId { get; set; }
        public string title { get; set; } = null!;
    }
}
