using Chillindo.Core.Data;
using Chillindo.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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

        private AccountTransactionResponse ErrorResponse(int accountNumber, string message)
        {
            return new AccountTransactionResponse
            {
                AccountNumber = accountNumber,
                Successful = false,
                Message = message
            };
        }

        private AccountTransactionResponse ConvertToResponse(Account account, string currency = null)
        {
            var response = new AccountTransactionResponse
            {
                AccountNumber = account.AccountNumber,
                Successful = true,
                Message = "Success",
            };

            if (currency == null)
            {
                response.AccountBalances = account.Balances.Select(b =>
                    new AccountBalanceResponse
                    {
                        Currency = b.Currency,
                        Balance = b.Balance
                    }).ToList();
            }
            else
            {
                response.Currency = currency;
                response.Balance = account.Balances.FirstOrDefault(b => b.Currency == currency)?.Balance ?? 0;
            }

            return response;
        }

        public async Task<AccountTransactionResponse> Balance(int accountNumber)
        {
            _logger.LogInformation($"Get Balance for account number: {accountNumber}");
            var account = await _db.Accounts
                .Include(a => a.Balances)
                .FirstOrDefaultAsync(acc => acc.AccountNumber == accountNumber);

            if (account == null)
                return ErrorResponse(accountNumber, $"Invalid Account Number: {accountNumber}");

            return ConvertToResponse(account);
        }

        public async Task<AccountTransactionResponse> Deposit(AccountTransactionRequest request)
        {
            try
            {
                _logger.LogInformation($"Deposit amount to account number: {request.AccountNumber}");

                var account = await _db.Accounts
                    .Include(a => a.Balances)
                    .FirstOrDefaultAsync(acc => acc.AccountNumber == request.AccountNumber);

                if (account == null)
                    return ErrorResponse(request.AccountNumber, $"Invalid Account Number: {request.AccountNumber}");

                var balanceWithCurr = account.Balances.FirstOrDefault(b => b.Currency == request.Currency);

                if (balanceWithCurr == null)
                    account.Balances.Add(new Core.Models.AccountBalance
                    {
                        Currency = request.Currency,
                        Balance = request.Amount
                    });
                else
                    balanceWithCurr.Balance += request.Amount;

                _db.TransactionHistories.Add(new TransactionHistory
                {
                    AccountNumber = request.AccountNumber,
                    TransactionType = TransactionType.Deposit,
                    Currency = request.Currency,
                    Amount = request.Amount,
                    TransactionTime = DateTime.Now
                });

                await _db.SaveChangesAsync();

                account = await _db.Accounts
                    .Include(a => a.Balances)
                    .FirstOrDefaultAsync(acc => acc.AccountNumber == request.AccountNumber);

                return ConvertToResponse(account, request.Currency);
            }
            catch (Exception ex)
            {
                return ErrorResponse(request.AccountNumber, ex.Message);
            }
        }

        public async Task<AccountTransactionResponse> Withdraw(AccountTransactionRequest request)
        {
            try
            {
                _logger.LogInformation($"Withdraw amount to account number: {request.AccountNumber}");

                var account = await _db.Accounts
                    .Include(a => a.Balances)
                    .FirstOrDefaultAsync(acc => acc.AccountNumber == request.AccountNumber);

                if (account == null)
                    return ErrorResponse(request.AccountNumber, $"Invalid Account Number: {request.AccountNumber}");

                var balanceWithCurr = account.Balances.FirstOrDefault(b => b.Currency == request.Currency);

                if (balanceWithCurr == null || balanceWithCurr.Balance < request.Amount)
                    return ErrorResponse(request.AccountNumber, $"Insufficient balance");

                balanceWithCurr.Balance -= request.Amount;

                _db.TransactionHistories.Add(new TransactionHistory
                {
                    AccountNumber = request.AccountNumber,
                    TransactionType = TransactionType.Deposit,
                    Currency = request.Currency,
                    Amount = request.Amount,
                    TransactionTime = DateTime.Now
                });

                await _db.SaveChangesAsync();

                account = await _db.Accounts
                    .Include(a => a.Balances)
                    .FirstOrDefaultAsync(acc => acc.AccountNumber == request.AccountNumber);

                return ConvertToResponse(account, request.Currency);
            }
            catch (Exception ex)
            {
                return ErrorResponse(request.AccountNumber, ex.Message);
            }
        }
    }
}