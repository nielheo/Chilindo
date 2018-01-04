using Chillindo.Core.Data;
using Chillindo.Core.Models;
using Chillindo.Data;
using Chillindo.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chillindo.Tests.Unit
{
    public partial class AccountRepository_Test
    {
        [TestMethod]
        public async Task DepositTHBToAccount1234()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "THB",
                Amount = 1000
            };

            var result = await _accountRepository.Deposit(request);
            
            Assert.AreEqual(true, result.Successful, "Deposit THB 1000 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Deposit THB 1000 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("THB", result.Currency, "Deposit THB 1000 to account number 1234 should successfully deposit to currency THB");
            Assert.AreEqual(6000, result.Balance, "Deposit THB 1000 to account number 1234, balance should be 6000");

            Assert.AreEqual(null, result.AccountBalances, "Deposit should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Deposit THB 1000 should not add new row to account balances (remain 2 rows)");
        }

        [TestMethod]
        public async Task DepositUSDToAccount1234()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "USD",
                Amount = 125
            };

            var result = await _accountRepository.Deposit(request);
            
            Assert.AreEqual(true, result.Successful, "Deposit USD 125 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Deposit USD 125 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("USD", result.Currency, "Deposit USD 125 to account number 1234 should successfully deposit to currency USD");
            Assert.AreEqual(375, result.Balance, "Deposit USD 125 to account number 1234, balance should be 375");

            Assert.AreEqual(null, result.AccountBalances, "Deposit should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Deposit USD 125 should not add new row to account balances (remain 2 rows)");
        }

        [TestMethod]
        public async Task DepositSGDToAccount1234()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1234,
                Currency = "SGD",
                Amount = 165
            };

            var result = await _accountRepository.Deposit(request);
            
            Assert.AreEqual(true, result.Successful, "Deposit SGD 165 to account number 1234 should be successful");
            Assert.AreEqual(1234, result.AccountNumber, "Deposit SGD 165 to account number 1234 should return AccuntNumber: 1234");
            Assert.AreEqual("SGD", result.Currency, "Deposit SGD 165 to account number 1234 should successfully deposit to currency SGD");
            Assert.AreEqual(165, result.Balance, "Deposit SGD 165 to account number 1234, balance should be 165");

            //Negative
            Assert.AreEqual(null, result.AccountBalances, "Deposit should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(1234);
            Assert.AreEqual(3, resultPost.AccountBalances.Count, "Deposit SGD 165 should add new row to account balances (become 3 rows)");
        }

        //-------------------------
        [TestMethod]
        public async Task DepositTHBToAccount3456()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 3456,
                Currency = "THB",
                Amount = 1200
            };

            var result = await _accountRepository.Deposit(request);
            
            Assert.AreEqual(true, result.Successful, "Deposit THB 1200 to account number 3456 should be successful");
            Assert.AreEqual(3456, result.AccountNumber, "Deposit THB 1200 to account number 3456 should return AccuntNumber: 1234");
            Assert.AreEqual("THB", result.Currency, "Deposit THB 1200 to account number 3456 should successfully deposit to currency THB");
            Assert.AreEqual(23700, result.Balance, "Deposit THB 1200 to account number 3456, balance should be 6000");

            Assert.AreEqual(null, result.AccountBalances, "Deposit should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(3456);
            Assert.AreEqual(1, resultPost.AccountBalances.Count, "Deposit THB 1200 should not add new row to account balances (remain 1 rows)");
        }

        [TestMethod]
        public async Task DepositUSDandSGDToAccount3456()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 3456,
                Currency = "USD",
                Amount = 372
            };

            var result = await _accountRepository.Deposit(request);
            
            Assert.AreEqual(true, result.Successful, "Deposit USD 372 to account number 3456 should be successful");
            Assert.AreEqual(3456, result.AccountNumber, "Deposit USD 372 to account number 3456 should return AccuntNumber: 1234");
            Assert.AreEqual("USD", result.Currency, "Deposit USD 372 to account number 3456 should successfully deposit to currency USD");
            Assert.AreEqual(372, result.Balance, "Deposit USD 372 to account number 3456, balance should be 375");

            Assert.AreEqual(null, result.AccountBalances, "Deposit should not return Account Balances items");

            var resultPost = await _accountRepository.Balance(3456);
            Assert.AreEqual(2, resultPost.AccountBalances.Count, "Deposit USD 372 should add new row to account balances (becomes 2 rows)");

            //deposit SGD
            request = new AccountTransactionRequest
            {
                AccountNumber = 3456,
                Currency = "SGD",
                Amount = 317
            };

            result = await _accountRepository.Deposit(request);

            Assert.AreEqual(true, result.Successful, "Deposit SGD 317 to account number 3456 should be successful");
            Assert.AreEqual(3456, result.AccountNumber, "Deposit SGD 317 to account number 3456 should return AccuntNumber: 1234");
            Assert.AreEqual("SGD", result.Currency, "Deposit SGD 317 to account number 3456 should successfully deposit to currency SGD");
            Assert.AreEqual(317, result.Balance, "Deposit SGD 317 to account number 3456, balance should be 165");

            //Negative
            Assert.AreEqual(null, result.AccountBalances, "Deposit should not return Account Balances items");

            resultPost = await _accountRepository.Balance(3456);
            Assert.AreEqual(3, resultPost.AccountBalances.Count, "Deposit SGD 165 should add new row to account balances (become 3 rows)");
        }

        [TestMethod]
        public async Task FailedDepositTHBToAccount1224()
        {
            AccountTransactionRequest request = new AccountTransactionRequest
            {
                AccountNumber = 1224,
                Currency = "THB",
                Amount = 1000
            };

            var result = await _accountRepository.Deposit(request);

            Assert.AreEqual(false, result.Successful, "Deposit THB 1000 to account number 1224 should be NOT successful");
            Assert.AreEqual(1224, result.AccountNumber, "Deposit THB 1000 to account number 1224 should return AccuntNumber: 1234");
            Assert.AreNotEqual("Successful", result.Message, "Deposit THB 1000 to account number 1224 should return error message");

            Assert.AreEqual(null, result.Currency, "Deposit THB 1000 to account number 1224 should return currency null (not successful)");
            Assert.AreEqual(null, result.Balance, "Deposit THB 1000 to account number 1224, should return null balance");

            Assert.AreEqual(null, result.AccountBalances, "Deposit should not return Account Balances items");
        }

    }
}