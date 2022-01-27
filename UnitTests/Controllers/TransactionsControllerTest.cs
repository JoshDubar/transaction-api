using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using TransactionAPI.Controllers;
using TransactionAPI.Data;
using TransactionAPI.DTOs;
using TransactionAPI.Models;
using TransactionAPI.Profiles;

namespace TransactionAPI.UnitTests.Controllers
{
    [TestClass]
    public class TransactionsControllerTest
    {
        private readonly List<Transaction> mockTransactions = new List<Transaction>()
        {
            new Transaction(3 , DateTime.Parse("2020-04-09T16:26:23Z"),  "Refund",  30.00,  TransactionStatus.Completed.ToString()),
            new Transaction(2 , DateTime.Parse("2020-04-01T12:47:23Z"),  "Amazon Online",   -30.00,  TransactionStatus.Completed.ToString()),
            new Transaction(1 , DateTime.Parse("2020-03-30T23:53:23Z"),  "Bank Deposit",    500.00,  TransactionStatus.Completed.ToString())
        };
        private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<TransactionsProfile>()));

        [TestMethod]
        public void GetTransactions()
        {
            var mockTransactionRepo = new Mock<ITransactionRepo>();
            mockTransactionRepo.Setup(x => x.GetTransactions()).Returns(mockTransactions);
            var controller = new TransactionsController(mockTransactionRepo.Object, _mapper);
            var response = controller.GetTransactions();

            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var responseAsOkObjectResult = ((OkObjectResult)response.Result).Value;

            Assert.IsInstanceOfType(responseAsOkObjectResult, typeof(IEnumerable<TransactionReadDTO>)); 
            var responseValue = (IEnumerable<TransactionReadDTO>)(responseAsOkObjectResult);
            var mappedResponseValue = _mapper.Map<IEnumerable<Transaction>>(responseValue);
            var transactionsFromController = (List<Transaction>)mappedResponseValue;

            Assert.AreEqual(transactionsFromController.Count, mockTransactions.Count);
        }

        private int transactionId;

        [TestMethod]
        public void GetTransactionById_Ok()
        {
            transactionId = 1;
            var mockTransactionRepo = new Mock<ITransactionRepo>();
            mockTransactionRepo.Setup(x => x.GetTransactionById(transactionId)).Returns(mockTransactions[2]);
            var controller = new TransactionsController(mockTransactionRepo.Object, _mapper);
            var response = controller.GetTransactionById(transactionId);

            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            var responseAsOkObjectResult = ((OkObjectResult)response.Result).Value;

            Assert.IsInstanceOfType(responseAsOkObjectResult, typeof(TransactionReadDTO));
            var responseValue = (TransactionReadDTO)(responseAsOkObjectResult);
            var mappedResponseValue = _mapper.Map<Transaction>(responseValue);

            Assert.AreEqual(mappedResponseValue, mockTransactions[2]);
        }

        [TestMethod]
        public void GetTransactionById_NotFound()
        {
            transactionId = 4;
            var mockTransactionRepo = new Mock<ITransactionRepo>();
            mockTransactionRepo.Setup(x => x.GetTransactionById(transactionId));
            var controller = new TransactionsController(mockTransactionRepo.Object, _mapper);
            var response = controller.GetTransactionById(transactionId);

            Assert.IsInstanceOfType(response.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateTransactionStatus_NotFound()
        {
            transactionId = 0;
            var mockTransactionRepo = new Mock<ITransactionRepo>();
            mockTransactionRepo.Setup(x => x.UpdateTransactionStatus(mockTransactions[2]));
            var controller = new TransactionsController(mockTransactionRepo.Object, _mapper);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns("token");
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var response = controller.UpdateTransactionStatus(transactionId, new JsonPatchDocument<TransactionUpdateDTO>());

            Assert.IsInstanceOfType(response.Result, typeof(NotFoundResult));
        }

        // I didn't manage to figure out how to mock the model validator
        // to return false on an invalid patch value. I would like to know how to but am
        // unable to find any resources which solve this problem.

        //[TestMethod]
        //public void UpdateTransactionStatus_ValidationProblem()
        //{
        //    transactionId = 0;
        //    var mockTransactionRepo = new Mock<ITransactionRepo>();
        //    mockTransactionRepo.Setup(x => x.GetTransactionById(transactionId)).Returns(mockTransactions[2]);
        //    var controller = new TransactionsController(mockTransactionRepo.Object, _mapper);
        //    var mockHttpContext = new Mock<HttpContext>();
        //    mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns("token");
        //    controller.ControllerContext.HttpContext = mockHttpContext.Object;

        //    var objectValidatorMock = new Mock<IObjectModelValidator>();
        //    objectValidatorMock.Setup(x => x.Validate(It.IsAny<ActionContext>(), It.IsAny<ValidationStateDictionary>(),
        //                                              It.IsAny<string>(), It.IsAny<object>()));
        //    controller.ObjectValidator = objectValidatorMock.Object;

        //    var patch = new JsonPatchDocument<TransactionUpdateDTO>();
        //    patch.Operations.Add(new Operation<TransactionUpdateDTO>("replace", "/status", null, null));
        //    var response = controller.UpdateTransactionStatus(transactionId, patch);

        //    Assert.IsInstanceOfType(response.Result, typeof(BadRequestResult));
        //}

        [TestMethod]
        public void UpdateTransactionStatus_NoContent()
        {
            transactionId = 0;
            var mockTransactionRepo = new Mock<ITransactionRepo>();
            mockTransactionRepo.Setup(x => x.GetTransactionById(transactionId)).Returns(mockTransactions[2]);
            var controller = new TransactionsController(mockTransactionRepo.Object, _mapper);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns("token");
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var objectValidatorMock = new Mock<IObjectModelValidator>();
            objectValidatorMock.Setup(x => x.Validate(It.IsAny<ActionContext>(), It.IsAny<ValidationStateDictionary>(),
                                                      It.IsAny<string>(), It.IsAny<object>()));
            controller.ObjectValidator = objectValidatorMock.Object;

            var patch = new JsonPatchDocument<TransactionUpdateDTO>();
            patch.Operations.Add(new Operation<TransactionUpdateDTO>("replace", "/status", null, "Completed"));
            var response = controller.UpdateTransactionStatus(transactionId, patch);

            Assert.IsInstanceOfType(response.Result, typeof(NoContentResult));
        }
    }
}
