using Chillindo.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chillindo.Data.Seeds
{
    public static class AccountSeedData
    {

        public static void EnsureSeedData(this ChillindoContext db)
        {
            db._logger.LogInformation("Seeding database");

            //Account 1
            var account1 = new Account { AccountNumber = 1234, IsActive = true };
            account1.Balances = new List<AccountBalance>
            {
                new AccountBalance
                {
                    AccountNumber = 1234,
                     Currency = "THB",
                     Balance = 15000
                },
                new AccountBalance
                {
                    AccountNumber = 1234,
                     Currency = "USD",
                     Balance = 250
                },
            };

            //Account 2
            var account2 = new Account { AccountNumber = 3456, IsActive = true };
            account2.Balances = new List<AccountBalance>
            {
                new AccountBalance
                {
                    AccountNumber = 3456,
                     Currency = "THB",
                     Balance = 25000
                },
                new AccountBalance
                {
                    AccountNumber = 3456,
                     Currency = "USD",
                     Balance = 0
                },
            };

            //Account 3
            var account3 = new Account { AccountNumber = 7890, IsActive = true };
            account3.Balances = new List<AccountBalance>
            {
                new AccountBalance
                {
                    AccountNumber = 7890,
                     Currency = "THB",
                     Balance = 0
                },
                new AccountBalance
                {
                    AccountNumber = 7890,
                     Currency = "USD",
                     Balance = 500
                },
            };

            var accounts = new List<Account> { account1, account2, account3 };

            if (!db.Accounts.Any())
            {
                db._logger.LogInformation("Seeding accounts");
                db.Accounts.AddRange(accounts);
                db.SaveChanges();
            }
            
        }
    }
}
