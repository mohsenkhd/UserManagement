using Application.RepositoryContracts.WalletAggregate;
using Common.Exceptions.UserManagement;
using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.WalletAggregate
{
    public class WalletRepository : IWalletRepository
    {
        private readonly UserManagementContext _context;
        IQueryable<Wallet> AllWallets;

        public async Task AddWalletTransaction(WalletTransaction tx)
        {
            await _context.WalletTransactions.AddAsync(tx);
            await _context.SaveChangesAsync();
        }

        public async Task<Wallet> GetWalletByUserId(long userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.IsDeleted == false && a.UserId == userId);
            return wallet ?? throw UserManagementExceptions.WalletNotFoundException;
        }

        public async Task<Wallet> GetWalletById(long walletId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == walletId);
            return wallet ?? throw UserManagementExceptions.WalletNotFoundException;
        }

        public async Task<Wallet> UpdateWallet(Wallet model)
        {
            var wallet = await GetWalletById(model.Id);
            if (wallet == null) throw UserManagementExceptions.WalletNotFoundException;

            wallet.Balance = model.Balance;
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }
    }
}
