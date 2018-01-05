using System.Threading.Tasks;

namespace Chilindo.Core.Data
{
    public interface IAccountRepository
    {
        Task<AccountTransactionResponse> Deposit(AccountTransactionRequest request);

        Task<AccountTransactionResponse> Withdraw(AccountTransactionRequest request);

        Task<AccountTransactionResponse> Balance(int accountId);
    }
}