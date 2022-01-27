using System.Collections.Generic;
using System.Linq;
using TransactionAPI.Models;

namespace TransactionAPI.Data
{
    public class DBTransactionRepo : ITransactionRepo
    {
        private readonly TransactionContext _context;

        public DBTransactionRepo(TransactionContext context)
        {
            _context = context;
        }

        public Transaction GetTransactionById(int id)
        {
            return _context.Transactions.FirstOrDefault(t => t.ID == id);
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            var transactions = _context.Transactions.ToList();
            transactions.Sort();
            return transactions;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void UpdateTransactionStatus(Transaction transaction)
        {
            
        }
    }
}
