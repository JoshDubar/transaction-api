using System.Collections.Generic;
using TransactionAPI.Models;

namespace TransactionAPI.Data
{
    public interface ITransactionRepo
    {
        IEnumerable<Transaction> GetTransactions();
        Transaction GetTransactionById(int id);
        void UpdateTransactionStatus(Transaction transaction);
        bool SaveChanges();
    }
}
