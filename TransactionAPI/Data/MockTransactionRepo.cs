using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using TransactionAPI.Models;

namespace TransactionAPI.Data
{
    public class MockTransactionRepo : ITransactionRepo
    {
        private readonly List<Transaction> INITIAL_TRANSACTIONS = new List<Transaction>()
            {
                new Transaction(11, DateTime.Parse("2020-09-08T16:34:23Z"),  "Bank Deposit",    500.00,  TransactionStatus.Completed.ToString()),
                new Transaction(10, DateTime.Parse("2020-09-08T09:02:23Z"),  "Transfer to James",   -20.00,  TransactionStatus.Pending.ToString()),
                new Transaction(9 , DateTime.Parse("2020-09-07T21:52:23Z"),  "ATM withdrawal",  -20.00,  TransactionStatus.Completed.ToString()),
                new Transaction(8 , DateTime.Parse("2020-09-06T10:32:23Z"),  "Google Subscription", -15.00,  TransactionStatus.Completed.ToString()),
                new Transaction(7 , DateTime.Parse("2020-09-06T07:33:23Z"),  "Apple Store", -2000.00,    TransactionStatus.Cancelled.ToString()),
                new Transaction(6 , DateTime.Parse("2020-08-23T17:39:23Z"),  "Mini Mart",   -23.76,  TransactionStatus.Completed.ToString()),
                new Transaction(5 , DateTime.Parse("2020-08-16T21:06:23Z"),  "Mini Mart",   -56.43, TransactionStatus.Completed.ToString()),
                new Transaction(4 , DateTime.Parse("2020-06-15T18:17:23Z"),  "Country Railways",    -167.78, TransactionStatus.Completed.ToString()),
                new Transaction(3 , DateTime.Parse("2020-04-09T16:26:23Z"),  "Refund",  30.00,  TransactionStatus.Completed.ToString()),
                new Transaction(2 , DateTime.Parse("2020-04-01T12:47:23Z"),  "Amazon Online",   -30.00,  TransactionStatus.Completed.ToString()),
                new Transaction(1 , DateTime.Parse("2020-03-30T23:53:23Z"),  "Bank Deposit",    500.00,  TransactionStatus.Completed.ToString())
            };

        private readonly List<Transaction> transactions;
        private readonly IMemoryCache _memoryCache;

        public MockTransactionRepo(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            if (!_memoryCache.TryGetValue("transactions", out List<Transaction> _))
            {
                transactions = INITIAL_TRANSACTIONS;
                _memoryCache.Set("transactions", transactions);
            } else
            {
                transactions = (List<Transaction>)_memoryCache.Get("transactions");
            }
        }

        public Transaction GetTransactionById(int id)
        {
            return transactions.FirstOrDefault(transaction => transaction.ID == id);
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            transactions.Sort();
            return transactions;
        }

        public bool SaveChanges()
        {
            return true;
        }

        public void UpdateTransactionStatus(Transaction transaction)
        {
            var index = transactions.FindIndex(trans => trans.ID == transaction.ID);
            transactions[index] = transaction;
            _memoryCache.Set("transactions", transactions);
        }
    }
}
