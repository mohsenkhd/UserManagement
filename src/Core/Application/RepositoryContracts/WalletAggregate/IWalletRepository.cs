using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryContracts.WalletAggregate
{
    public interface IWalletRepository
    {
        Task<Wallet> GetWalletByUserId(long userId);
        Task<Wallet> UpdateWallet(Wallet model);
        Task AddWalletTransaction(WalletTransaction tx);
    }
}
