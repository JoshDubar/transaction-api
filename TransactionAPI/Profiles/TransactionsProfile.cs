using AutoMapper;
using TransactionAPI.DTOs;
using TransactionAPI.Models;

namespace TransactionAPI.Profiles
{
    public class TransactionsProfile : Profile
    {
        public TransactionsProfile()
        {
            CreateMap<Transaction, TransactionReadDTO>();
            CreateMap<TransactionReadDTO, Transaction>();
            CreateMap<Transaction, TransactionUpdateDTO>();
            CreateMap<TransactionUpdateDTO, Transaction>();
        }
    }
}
