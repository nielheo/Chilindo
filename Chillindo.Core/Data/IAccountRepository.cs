using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chillindo.Core.Data
{
    public interface IAccountRepository
    {
        Task<AccountTransactionResponse> Deposit(AccountTransactionRequest request);
        Task<AccountTransactionResponse> Withdraw(AccountTransactionRequest request);
        Task<AccountTransactionResponse> Balance(int accountId);
    }
}
