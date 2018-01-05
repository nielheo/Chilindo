using Chilindo.Core.Models;
using Chilindo.Data;
using Chilindo.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chilindo.Tests.Unit
{
    [TestClass]
    public partial class AccountRepository_Test
    {
        private readonly AccountRepository _accountRepository;

        public AccountRepository_Test()
        {
            var dbLogger = new Mock<ILogger<ChilindoContext>>();
            // Given
            // https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory
            var options = new DbContextOptionsBuilder<ChilindoContext>()
                .UseInMemoryDatabase(databaseName: "Chilindo")
                .Options;

            using (var context = new ChilindoContext(options, dbLogger.Object))
            {
                context.Database.EnsureDeleted();

                context.Accounts.Add(new Account
                {
                    AccountNumber = 1234,
                    IsActive = true,
                    Balances = new List<AccountBalance>
                    {
                        new AccountBalance { AccountNumber = 1234, Currency = "THB", Balance = 5000 },
                        new AccountBalance { AccountNumber = 1234, Currency = "USD", Balance = 250 }
                    }
                });

                context.Accounts.Add(new Account
                {
                    AccountNumber = 3456,
                    IsActive = true,
                    Balances = new List<AccountBalance>
                    {
                        new AccountBalance { AccountNumber = 3456, Currency = "THB", Balance = 22500 }
                    }
                });

                context.Accounts.Add(new Account
                {
                    AccountNumber = 7890,
                    IsActive = true,
                    Balances = new List<AccountBalance>
                    {
                        new AccountBalance { AccountNumber = 7890, Currency = "USD", Balance = 750 }
                    }
                });

                context.SaveChanges();
            }
            var starWarsContext = new ChilindoContext(options, dbLogger.Object);
            var repoLogger = new Mock<ILogger<AccountRepository>>();
            _accountRepository = new AccountRepository(starWarsContext, repoLogger.Object);
        }

        [TestMethod]
        public async Task ReturnDataGivenAccountNumber1234()
        {
            var result = await _accountRepository.Balance(1234);
            List<string> validCurrs = new List<string> { "USD", "THB" };

            Assert.AreEqual(true, result.Successful, "Balance for account number 1234 should success");
            Assert.AreEqual(1234, result.AccountNumber, "Balance for account number 1234 should have AccuntNumber: 1234");
            Assert.AreEqual(2, result.AccountBalances.Count, "Balance for account number 1234 should return 2 items of AccountBalance");
            Assert.AreEqual(1, result.AccountBalances.Where(a => a.Currency == "THB").Count(), "Balance for account number 1234 should return 1 items of AccountBalance with currency: THB");
            Assert.AreEqual(1, result.AccountBalances.Where(a => a.Currency == "USD").Count(), "Balance for account number 1234 should return 1 items of AccountBalance with currency: USD");
            Assert.AreEqual(5000, result.AccountBalances.First(a => a.Currency == "THB").Balance, "Balance for account number 1234 should return balance 22500 for currency: THB");
            Assert.AreEqual(250, result.AccountBalances.First(a => a.Currency == "USD").Balance, "Balance for account number 1234 should return balance 0 for currency: USD");

            //Negative
            Assert.AreEqual(0, result.AccountBalances.Where(a => !validCurrs.Contains(a.Currency)).Count(), "Balance for account number 1234 should return no items of AccountBalance with currency except THB, USD");
            Assert.AreEqual(null, result.Currency, "Balance for account number 1234 should not return currency in root");
            Assert.AreEqual(null, result.Balance, "Balance for account number 1234 should not amount currency in root");
        }

        [TestMethod]
        public async Task ReturnDataGivenAccountNumber3456()
        {
            var result = await _accountRepository.Balance(3456);
            List<string> validCurrs = new List<string> { "USD", "THB" };

            Assert.AreEqual(true, result.Successful, "Balance for account number 3456 should success");
            Assert.AreEqual(3456, result.AccountNumber, "Balance for account number 3456 should have AccuntNumber: 1234");
            Assert.AreEqual(1, result.AccountBalances.Count, "Balance for account number 3456 should return 1 items of AccountBalance");
            Assert.AreEqual(1, result.AccountBalances.Where(a => a.Currency == "THB").Count(), "Balance for account number 3456 should return 1 item of AccountBalance with currency: THB");
            Assert.AreEqual(0, result.AccountBalances.Where(a => a.Currency == "USD").Count(), "Balance for account number 3456 should return 0 item of AccountBalance with currency: USD");
            Assert.AreEqual(22500, result.AccountBalances.First(a => a.Currency == "THB").Balance, "Balance for account number 3456 should return balance 5000 for currency: THB");
            Assert.AreEqual(null, result.AccountBalances.FirstOrDefault(a => a.Currency == "USD"), "Balance for account number 3456 should return balance 250 for currency: USD");

            //Negative
            Assert.AreEqual(0, result.AccountBalances.Where(a => !validCurrs.Contains(a.Currency)).Count(), "Balance for account number 1234 should return no items of AccountBalance with currency except THB, USD");
            Assert.AreEqual(null, result.Currency, "Balance for account number 1234 should not return currency in root");
            Assert.AreEqual(null, result.Balance, "Balance for account number 1234 should not amount currency in root");
        }

        [TestMethod]
        public async Task ReturnDataGivenAccountNumber7890()
        {
            var result = await _accountRepository.Balance(7890);
            List<string> validCurrs = new List<string> { "USD", "THB" };

            Assert.AreEqual(true, result.Successful, "Balance for account number 1234 should success");
            Assert.AreEqual(7890, result.AccountNumber, "Balance for account number 1234 should have AccuntNumber: 1234");
            Assert.AreEqual(1, result.AccountBalances.Count, "Balance for account number 1234 should return 2 items of AccountBalance");
            Assert.AreEqual(0, result.AccountBalances.Where(a => a.Currency == "THB").Count(), "Balance for account number 1234 should return 1 items of AccountBalance with currency: THB");
            Assert.AreEqual(1, result.AccountBalances.Where(a => a.Currency == "USD").Count(), "Balance for account number 1234 should return 1 items of AccountBalance with currency: USD");
            Assert.AreEqual(null, result.AccountBalances.FirstOrDefault(a => a.Currency == "THB"), "Balance for account number 1234 should return balance 5000 for currency: THB");
            Assert.AreEqual(750, result.AccountBalances.First(a => a.Currency == "USD").Balance, "Balance for account number 1234 should return balance 250 for currency: USD");

            //Negative
            Assert.AreEqual(0, result.AccountBalances.Where(a => !validCurrs.Contains(a.Currency)).Count(), "Balance for account number 1234 should return no items of AccountBalance with currency except THB, USD");
            Assert.AreEqual(null, result.Currency, "Balance for account number 1234 should not return currency in root");
            Assert.AreEqual(null, result.Balance, "Balance for account number 1234 should not amount currency in root");
        }

        [TestMethod]
        public async Task NotReturnDataGivenAccountNumber1223()
        {
            var result = await _accountRepository.Balance(1223);
            List<string> validCurrs = new List<string> { "USD", "THB" };

            Assert.AreEqual(false, result.Successful, "Balance for account number 1223 should success");
            Assert.AreEqual("Invalid Account Number: 1223", result.Message);

            ////Negative
            Assert.AreEqual(null, result.Currency, "Balance for account number 1234 should not return currency in root");
            Assert.AreEqual(null, result.Balance, "Balance for account number 1234 should not amount currency in root");
            Assert.AreEqual(null, result.AccountBalances, "Balance for account number 1234 should not return account balances");
        }
    }
}