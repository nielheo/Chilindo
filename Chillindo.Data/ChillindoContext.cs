using Chillindo.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chillindo.Data
{
    public class ChillindoContext : DbContext
    {
        public readonly ILogger _logger;
        private bool _migrations;

        public ChillindoContext()
        {
            _migrations = true;
        }

        public ChillindoContext(DbContextOptions options, ILogger<ChillindoContext> logger)
            : base(options)
        {
            _logger = logger;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_migrations)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb);Initial Catalog=Chillindo;Integrated Security=SSPI;integrated security=true;MultipleActiveResultSets=True;",
                    b=>b.MigrationsAssembly("Chillindo.Api"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // https://docs.microsoft.com/en-us/ef/core/modeling/relationships
            // http://stackoverflow.com/questions/38520695/multiple-relationships-to-the-same-table-in-ef7core

            // accounts
            modelBuilder.Entity<Account>().HasKey(c => c.AccountNumber);
            modelBuilder.Entity<Account>().Property(e => e.AccountNumber).ValueGeneratedNever();

            // account balances 
            modelBuilder.Entity<AccountBalance>().HasKey(c => c.Id);
            modelBuilder.Entity<AccountBalance>().Property(e => e.Id).ValueGeneratedOnAdd();
            
            // Acount Balance
            modelBuilder.Entity<AccountBalance>()
                .HasOne(h => h.Account)
                .WithMany(p => p.Balances)
                .HasForeignKey(h => h.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountBalance> AccountBalances { get; set; }
    }
}
