using Application.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountAggregate
{
    public class GetTokenViewModel : MainRes
    {
        public string Token { get; set; } = null!;
    }
}
