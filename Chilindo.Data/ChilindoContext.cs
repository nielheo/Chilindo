using Chilindo.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chilindo.Data
{
    public class ChilindoContext : DbContext
    {
        public readonly ILogger<ChilindoContext> _logger;
        public readonly DbContextOptions _options;
        private bool _migrations;

        public ChilindoContext()
        {
            _migrations = false;
        }

        public ChilindoContext(DbContextOptions options, ILogger<ChilindoContext> logger)
            : base(options)
        {
            _options = options;
            _logger = logger;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_migrations)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb);Initial Catalog=Chilindo;Integrated Security=SSPI;integrated security=true;MultipleActiveResultSets=True;",
                    b => b.MigrationsAssembly("Chilindo.Api"));
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

            modelBuilder.Entity<AccountBalance>()
                .Property(a => a.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            //Transaction History
            modelBuilder.Entity<TransactionHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<TransactionHistory>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<TransactionHistory>()
                .HasOne(h => h.Account)
                .WithMany(p => p.TransactionHistories)
                .HasForeignKey(h => h.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountBalance> AccountBalances { get; set; }
        public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }
    }
}