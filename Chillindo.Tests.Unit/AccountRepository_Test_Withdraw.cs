using Chillindo.Core.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Chillindo.Tests.Unit
{
    public partial class AccountRepository_Test
    {
        [TestMethod]
        public async Task WithdrawTHBFromAccount1234()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 1000
            };

            var result = await _accountRepository.Withdraw(request);

            Assert.AreEqual(true, result.Successful, "Withdraw THB 1000 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Withdraw THB 1000 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("THB", result.Currency, "Withdraw THB 1000 to account number 1234 should successfully deposit to currency THB");
            Assert.AreEqual(4000, result.Balance, "Withdraw THB 1000 to account number 1234, balance should be 4000");

            Assert.AreEqual(null, result.AccountBalances, "Withdraw should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Withdraw THB 1000 should not add new row to account balances (remain 2 rows)");
        }

        [TestMethod]
        public async Task WithdrawUSDToAccount1234()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "USD",
                Amount = 125
            };

            var result = await _accountRepository.Withdraw(request);

            Assert.AreEqual(true, result.Successful, "Withdraw USD 125 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Withdraw USD 125 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("USD", result.Currency, "Withdraw USD 125 to account number 1234 should successfully deposit to currency USD");
            Assert.AreEqual(125, result.Balance, "Withdraw USD 125 to account number 1234, balance should be 125");

            Assert.AreEqual(null, result.AccountBalances, "Withdraw should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Withdraw USD 125 should not add new row to account balances (remain 2 rows)");
        }

        [TestMethod]
        public async Task FailedWithdrawSGDToAccount1234()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "SGD",
                Amount = 165
            };

            var result = await _accountRepository.Withdraw(request);

            Assert.AreEqual(false, result.Successful, "Withdraw SGD 165 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Withdraw SGD 165 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreNotEqual("Successful", result.Message, "Withdraw SGD 165 to account number 1234 should not return successful message");
            Assert.AreEqual(null, result.Currency, "Withdraw SGD 165 to account number 1234 should return null as currency");
            Assert.AreEqual(null, result.Balance, "Withdraw SGD 165 to account number 1234, should return null as balance");

            //Negative
            Assert.AreEqual(null, result.AccountBalances, "Withdraw should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Withdraw SGD 165 should not add new row to account balances (remain 2 rows)");
        }

        [TestMethod]
        public async Task WithdrawTHB5000FromAccount1234()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 5000
            };

            var result = await _accountRepository.Withdraw(request);

            Assert.AreEqual(true, result.Successful, "Withdraw THB 5000 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Withdraw THB 5000 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("THB", result.Currency, "Withdraw THB 5000 to account number 1234 should successfully deposit to currency THB");
            Assert.AreEqual(0, result.Balance, "Withdraw THB 5000 to account number 1234, balance should be 0");

            Assert.AreEqual(null, result.AccountBalances, "Withdraw should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Withdraw THB 5000 should not add new row to account balances (remain 2 rows)");
        }

        [TestMethod]
        public async Task FailedWithdrawTHB5001FromAccount1234_InsufficienceBalance()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 5001
            };

            var result = await _accountRepository.Withdraw(request);

            Assert.AreEqual(false, result.Successful, "Withdraw THB 5001 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Withdraw THB 5001 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("Insufficient balance", result.Message, "Withdraw THB 5001 to account number 1234 should not return successful message");
            Assert.AreEqual(null, result.Currency, "Withdraw THB 5001 to account number 1234 should return null as currency");
            Assert.AreEqual(null, result.Balance, "Withdraw THB 5001 to account number 1234, should return null as balance");

            Assert.AreEqual(null, result.AccountBalances, "Withdraw should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Withdraw THB 5001 should not add new row to account balances (remain 2 rows)");
        }

        [TestMethod]
        public async Task FailedWithdrawTHB7000FromAccount1234_InsufficienceBalance()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 7000
            };

            var result = await _accountRepository.Withdraw(request);

            Assert.AreEqual(false, result.Successful, "Withdraw THB 7000 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Withdraw THB 7000 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("Insufficient balance", result.Message, "Withdraw THB 7000 to account number 1234 should not return successful message");
            Assert.AreEqual(null, result.Currency, "Withdraw THB 7000 to account number 1234 should return null as currency");
            Assert.AreEqual(null, result.Balance, "Withdraw THB 7000 to account number 1234, should return null as balance");

            Assert.AreEqual(null, result.AccountBalances, "Withdraw should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Withdraw THB 7000 should not add new row to account balances (remain 2 rows)");
        }
    }
}