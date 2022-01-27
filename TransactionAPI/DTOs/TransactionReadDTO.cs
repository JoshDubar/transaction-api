using System;

namespace TransactionAPI.DTOs
{
    public class TransactionReadDTO
    {
        public int ID { get; set; }
        public DateTime Datetime { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
    }
}
