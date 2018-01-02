using System;
using System.Collections.Generic;
using System.Text;

namespace Chillindo.Core.Data
{
    public class AccountTransactionRequest
    {
        public int AccountNumber { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
