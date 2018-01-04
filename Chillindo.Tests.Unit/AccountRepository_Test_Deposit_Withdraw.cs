using Chillindo.Core.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Chillindo.Tests.Unit
{
    public partial class AccountRepository_Test
    {
        [TestMethod]
        public async Task DepositWithdrawTHBFromAccount1234()
        {
            //Deposit THB 1000
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 1000
            };

            var result = await _accountRepository.Deposit(request);
            Assert.AreEqual(true, result.Successful, "Deposit THB 1000 to account number 1234 should be successful");

            //Withdraw THB 6000 afterward
            request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 6000
            };

            result = await _accountRepository.Withdraw(request);

            Assert.AreEqual(true, result.Successful, "Withdraw THB 6000 after deposit THb 1000 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Withdraw THB 6000 after deposit THb 1000 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("THB", result.Currency, "Withdraw THB 6000 after deposit THb 1000 to account number 1234 should successfully deposit to currency THB");
            Assert.AreEqual(0, result.Balance, "Withdraw THB 6000 after deposit THb 1000 to account number 1234, balance should be 0");

            Assert.AreEqual(null, result.AccountBalances, "Withdraw should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Withdraw THB 6000 after deposit THb 1000 should not add new row to account balances (remain 2 rows)");
        }

        [TestMethod]
        public async Task FailedWithdrawTwoTimesTHBFromAccount1234()
        {
            //withdraw THB 1000
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 3000
            };

            var result = await _accountRepository.Withdraw(request);
            Assert.AreEqual(true, result.Successful, "withdraw THB 3000 to account number 1234 should be successful");

            //Withdraw another THB 2500 afterward
            request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 2500
            };

            result = await _accountRepository.Withdraw(request);

            Assert.AreEqual(false, result.Successful, "Withdraw THB 2500 after withdraw THB 3000 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Withdraw THB 2500 after withdraw THB 3000 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("Insufficient balance", result.Message, "Withdraw THB 2500 after withdraw THB 3000 to account number 1234 should not return successful message");
            Assert.AreEqual(null, result.Currency, "Withdraw THB 2500 after withdraw THB 3000 to account number 1234 should return null as currency");
            Assert.AreEqual(null, result.Balance, "Withdraw THB 2500 after withdraw THB 3000 to account number 1234 should return null as balance");

            Assert.AreEqual(null, result.AccountBalances, "Withdraw should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Withdraw THB 2500 after withdraw THB 3000 should not add new row to account balances (remain 2 rows)");
        }

        [TestMethod]
        public async Task FailedDepositUSDWithdrawTHBFromAccount1234()
        {
            //Deposit USD 1000
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "USD",
                Amount = 1000
            };

            var result = await _accountRepository.Deposit(request);
            Assert.AreEqual(true, result.Successful, "Deposit USD 1000 to account number 1234 should be successful");

            //Withdraw THB 6000 afterward
            request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 6000
            };

            result = await _accountRepository.Withdraw(request);

            Assert.AreEqual(false, result.Successful, "Withdraw THB 6000 after deposit USD 1000 to account number 1234 should not be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Withdraw THB 6000 after deposit USD 1000 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual(null, result.Currency, "Withdraw THB 6000 after deposit USD 1000 to account number 1234 should return null as currency");
            Assert.AreEqual(null, result.Balance, "Withdraw THB 6000 after deposit USD 1000 to account number 1234 should return null as balance");

            Assert.AreEqual(null, result.AccountBalances, "Withdraw should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Withdraw THB 6000 after deposit USD 1000 should not add new row to account balances (remain 2 rows)");
        }
    }
}