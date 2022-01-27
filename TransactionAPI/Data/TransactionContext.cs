using Microsoft.EntityFrameworkCore;
using TransactionAPI.Models;

namespace TransactionAPI.Data
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> opt) : base(opt)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
