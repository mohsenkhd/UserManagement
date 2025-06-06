using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Wallet : Domain.Base.Base
    {
        public long UserId { get; set; }
        public decimal Balance { get; set; }

        public User User { get; set; }
        public ICollection<WalletTransaction> Transactions { get; set; }
    }
}