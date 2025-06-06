using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.ViewModels.WalletAggregate
{
    public class WalletTransactionViewModel
    {
        public decimal Amount { get; set; }
        public WalletTransactionType Type { get; set; }
        public string TypeName
        {
            get
            {
                switch (Type)
                {
                    case WalletTransactionType.Payment:
                        return "پرداخت";
                    case WalletTransactionType.Withdrawal:
                        return "برداشت";
                    case WalletTransactionType.Refund:
                        return "برگشت";
                    default:
                        return "پرداخت";
                }
            }
        }
    }
}