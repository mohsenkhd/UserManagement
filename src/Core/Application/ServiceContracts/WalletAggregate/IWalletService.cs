using Application.ViewModels.WalletAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceContracts.WalletAggregate
{
    public interface IWalletService
    {
        Task<bool> PayInvoice(long userId, long invoiceId);
        Task<WalletViewModel> GetWalletTransactionByUserId(long userId);
    }
}
