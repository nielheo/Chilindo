﻿using System.ComponentModel.DataAnnotations;

namespace Chilindo.Core.Models
{
    public class AccountBalance
    {
        public int Id { get; set; }

        public int AccountNumber { get; set; }
        public Account Account { get; set; }

        public string Currency { get; set; }
        public decimal Balance { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}