using System.Collections.Generic;

namespace Chillindo.Core.Models
{
    public class Account
    {
        public int AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AccountBalance> Balances { get; set; }
        public ICollection<TransactionHistory> TransactionHistories { get; set; }
    }
}