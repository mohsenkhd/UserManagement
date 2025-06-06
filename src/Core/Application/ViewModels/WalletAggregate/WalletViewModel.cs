using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.WalletAggregate
{
    public class WalletViewModel
    {
        public long WalletId { get; set; }
        public long UserId { get; set; }
        public decimal Balance { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Phone { get; set; } = null!;
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
        public long? CustomerNumber { get; set; }
        public List<WalletTransactionViewModel> Transactions { get; set; }

        public static explicit operator WalletViewModel(Wallet wallet)
        {
            return new WalletViewModel
            {
                WalletId = wallet.Id,
                Balance = wallet.Balance,
                UserId = wallet.UserId,
                FirstName = wallet.User.FirstName,
                LastName = wallet.User.LastName,
                Phone = wallet.User.Phone,
                NationalCode = wallet.User.NationalCode,
                Email = wallet.User.Email,
                CustomerNumber = wallet.User.CustomerNumber,
                Transactions = new List<WalletTransactionViewModel>(wallet.Transactions.Select(t => new WalletTransactionViewModel
                {
                    Amount = t.Amount,
                    Type = t.Type
                }))
            };
        }
    }
}
