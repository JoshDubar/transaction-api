using Microsoft.AspNetCore.Mvc;
using TransactionAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using AutoMapper;
using TransactionAPI.DTOs;

namespace TransactionAPI.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepo _repository;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TransactionReadDTO>> GetTransactions()
        {
            var transactions = _repository.GetTransactions();
            return Ok(_mapper.Map<IEnumerable<TransactionReadDTO>>(transactions));
        }

        [HttpGet("{id}")]
        public ActionResult<TransactionReadDTO> GetTransactionById(int id)
        {
            var transaction = _repository.GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TransactionReadDTO>(transaction));

        }

        [RequireAuthHeader]
        [HttpPatch("{id}")]
        public ActionResult<TransactionReadDTO> UpdateTransactionStatus(int id, JsonPatchDocument<TransactionUpdateDTO> patchDoc)
        {
            var transactionFromRepo = _repository.GetTransactionById(id);
            if (transactionFromRepo == null)
            {
                return NotFound();
            }
            var transactionModel = _mapper.Map<TransactionUpdateDTO>(transactionFromRepo);
            patchDoc.ApplyTo(transactionModel, ModelState);
            if (!TryValidateModel(transactionModel))
            {
                return ValidationProblem();
            }
            _mapper.Map(transactionModel, transactionFromRepo);
            _repository.UpdateTransactionStatus(transactionFromRepo);
            _repository.SaveChanges(); 
            return NoContent();
        }
    }
}
