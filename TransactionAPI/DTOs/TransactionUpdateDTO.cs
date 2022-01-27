using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionAPI.DTOs
{
    public class TransactionUpdateDTO
    {

        [Required]
        public string Status { get; set; }
    }
}
