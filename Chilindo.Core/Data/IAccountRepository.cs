using System.Threading.Tasks;

namespace Chilindo.Core.Data
{
    public interface IAccountRepository
    {
        Task<AccountTransactionResponse> Deposit(AccountTransactionRequest request, int maxRetry = 10, int Retry = 1);

        Task<AccountTransactionResponse> Withdraw(AccountTransactionRequest request, int maxRetry = 10, int Retry = 1);

        Task<AccountTransactionResponse> Balance(int accountId);
    }
}