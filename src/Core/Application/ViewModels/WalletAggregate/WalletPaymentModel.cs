using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.WalletAggregate
{
    public class WalletPaymentModel
    {
        public long UserId { get; set; }
        public long InvoiceId { get; set; }
    }
}
