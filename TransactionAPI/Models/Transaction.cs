using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TransactionAPI.Models
{
    public enum TransactionStatus
    {
        Pending,
        Completed,
        Cancelled
    }

    public class Transaction : IComparable<Transaction>
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public DateTime Datetime { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string Status { get; set; }

        public Transaction(int ID, DateTime Datetime, string Description, double Amount, string Status)
        {
            this.ID = ID;
            this.Datetime = Datetime;
            this.Description = Description;
            this.Amount = Amount;
            this.Status = Status;
        }

        public override string ToString()
        {
            return String.Format("ID: {0}, Datetime: {1}, Description: {2}, Amount: {3}, Status: {4}", ID, Datetime, Description, Amount, Status);  
        }

        public int CompareTo([AllowNull] Transaction other)
        {
            return other.Datetime.CompareTo(this.Datetime);
        }

        public override bool Equals(object obj)
        {
            return obj is Transaction transaction &&
                   ID == transaction.ID &&
                   Datetime == transaction.Datetime &&
                   Description == transaction.Description &&
                   Amount == transaction.Amount &&
                   Status == transaction.Status;
        }
    }
}
