using Application.RepositoryContracts.InvoiceAggregate;
using Application.RepositoryContracts.WalletAggregate;
using Application.ServiceContracts.WalletAggregate;
using Application.ViewModels.WalletAggregate;
using Common.Exceptions.UserManagement;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.WalletAggregate.WalletService;

namespace Service.WalletAggregate
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepo;
        private readonly IInvoiceRepository _invoiceRepo;

        public WalletService(IWalletRepository walletRepo, IInvoiceRepository invoiceRepo)
        {
            _walletRepo = walletRepo;
            _invoiceRepo = invoiceRepo;
        }

        public async Task<bool> PayInvoice(long userId, long invoiceId)
        {
            var wallet = await _walletRepo.GetWalletByUserId(userId);
            var invoice = await _invoiceRepo.GetInvoiceById(invoiceId);

            if (wallet == null)
                throw UserManagementExceptions.WalletNotFoundException;

            if (invoice == null)
                throw UserManagementExceptions.InvoiceNotFoundException;

            if (invoice.Status != InvoiceStatus.Pending)
                throw new Exception("فاکتور در وضعیت پیش نویس نمی باشد");

            if (wallet.Balance < invoice.Amount)
                throw new Exception("موجودی کیف پول برای پرداخت کافی نیست");

            wallet.Balance -= invoice.Amount;
            invoice.Status = InvoiceStatus.Paid;

            await _walletRepo.AddWalletTransaction(new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = -invoice.Amount,
                Timestamp = DateTime.UtcNow,
                Type = WalletTransactionType.Payment
            });

            await _walletRepo.UpdateWallet(wallet);
            await _invoiceRepo.UpdateInvoice(invoice);

            return true;
        }

        public async Task<WalletViewModel> GetWalletTransactionByUserId(long userId)
        {
            var wallet = await _walletRepo.GetWalletByUserId(userId);
            return (WalletViewModel)wallet;
        }
    }
}
