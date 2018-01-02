using Chillindo.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillindo.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private ChillindoContext _db { get; set; }
        private readonly ILogger _logger;

        public AccountRepository(ChillindoContext db, ILogger<AccountRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AccountTransactionResponse> Balance(int accountNumber)
        {
            _logger.LogInformation($"Get Balance for account number: {accountNumber}");
            var account = await _db.Accounts
                .Include(a => a.Balances)
                .FirstOrDefaultAsync(acc => acc.AccountNumber == accountNumber);

            if (account == null)
                return new AccountTransactionResponse
                {
                    AccountNumber = accountNumber,
                    Successful = false,
                    Message = $"Cannot found Account with Number: {accountNumber}"
                };

            return new AccountTransactionResponse
            {
                AccountNumber = accountNumber,
                Successful = true,
                Message = "Success",
                AccountBalances = account.Balances.Select(b =>
                    new AccountBalanceResponse
                    {
                        Currency = b.Currency,
                        Balance = b.Balance
                    }
                ).ToList()
            };
        }

        public async Task<AccountTransactionResponse> Deposit(AccountTransactionRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountTransactionResponse> Withdraw(AccountTransactionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
