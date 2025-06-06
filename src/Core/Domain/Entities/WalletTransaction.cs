using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WalletTransaction: Domain.Base.Base
    {
        public long WalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public WalletTransactionType Type { get; set; }
        public Wallet Wallet { get; set; }
    }
}
