using System.Collections.Generic;

namespace Chillindo.Core.Data
{
    public class AccountBalanceResponse
    {
        public string Currency { get; set; }
        public decimal Balance { get; set; }
    }

    public class AccountTransactionResponse
    {
        public int AccountNumber { get; set; }
        public bool Successful { get; set; }
        public decimal? Balance { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }
        public List<AccountBalanceResponse> AccountBalances { get; set; }
    }
}