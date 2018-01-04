using System;

namespace Chillindo.Core.Models
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2
    }

    public class TransactionHistory
    {
        public int Id { get; set; }

        public int AccountNumber { get; set; }
        public Account Account { get; set; }

        public TransactionType TransactionType { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }

        public DateTime TransactionTime { get; set; }
    }
}