using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopUpService.Models;
using TopUpService.Repositories;
using TopUpService.Services;

namespace TopUpService.Tests
{
    [TestClass]
    public class TransactionServiceTests
    {
        [TestMethod]
        public async Task GetAllTransactionAsync_Should_Return_Transactions()
        {
            var userId = Guid.NewGuid();
            var beneficiaries = new List<Beneficiary>
        {
            new Beneficiary { BenificiaryId = new Guid("2d3f71e1-5b36-4dfe-81c0-7cdba2c9e87e") },
            new Beneficiary { BenificiaryId = new Guid("33a49bd6-6286-469d-b3da-62b944914a03") }
        };

            var mockBeneficiaryRepository = new Mock<IBenificiaryRepository>();
            mockBeneficiaryRepository.Setup(repo => repo.GetAllBeneficiariesAsync(userId))
                .ReturnsAsync(beneficiaries);

            var transactions = new List<UserTransaction>
        {
            new UserTransaction { TransactionId = new Guid("deab8f41-dd1f-45c3-8ca4-d49603dd4ddc"), TopUpOption = new TopUpOption { OptionName = "AED 100" } },
            new UserTransaction { TransactionId = new Guid("ba57625f-b62b-4d32-93d6-674fef7f56e4"), TopUpOption = new TopUpOption { OptionName = "AED 50" } }
        };

            var mockTransactionRepository = new Mock<ITransactionRepository>();
            mockTransactionRepository.Setup(repo => repo.GetAllTransactionAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(transactions);

            var transactionService = new TransactionService(
                mockTransactionRepository.Object,
                null,
                mockBeneficiaryRepository.Object
            );

            // Act
            var result = await transactionService.GetAllTransactionAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task TopUpTransaction_UnverifiedUserMonthlyLimit_ThrowsException()
        {
            var userId = Guid.NewGuid();
            var beneficiaries = new List<Beneficiary>
        {
            new Beneficiary { BenificiaryId = new Guid("2d3f71e1-5b36-4dfe-81c0-7cdba2c9e87e") },
            new Beneficiary { BenificiaryId = new Guid("33a49bd6-6286-469d-b3da-62b944914a03") }
        };

            var mockBeneficiaryRepository = new Mock<IBenificiaryRepository>();
            mockBeneficiaryRepository.Setup(repo => repo.GetAllBeneficiariesAsync(userId))
                .ReturnsAsync(beneficiaries);

            var transactions = new List<UserTransaction>
        {
            new UserTransaction { TransactionId = new Guid("deab8f41-dd1f-45c3-8ca4-d49603dd4ddc"), TopUpOption = new TopUpOption { OptionName = "AED 100" } },
            new UserTransaction { TransactionId = new Guid("ba57625f-b62b-4d32-93d6-674fef7f56e4"), TopUpOption = new TopUpOption { OptionName = "AED 50" } }
        };

            var mockTransactionRepository = new Mock<ITransactionRepository>();
            mockTransactionRepository.Setup(repo => repo.GetAllTransactionAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(transactions);

            var transactionService = new TransactionService(
                mockTransactionRepository.Object,
                null,
                mockBeneficiaryRepository.Object
            );

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => transactionService.TopUpTransaction(userId, transactions[0]));
        }

        [TestMethod]
        public async Task TopUpTransaction_VerifiedUserMonthlyLimit_ThrowsException()
        {
            var userId = Guid.NewGuid();
            var beneficiaries = new List<Beneficiary>
        {
            new Beneficiary { BenificiaryId = new Guid("2d3f71e1-5b36-4dfe-81c0-7cdba2c9e87e") },
            new Beneficiary { BenificiaryId = new Guid("33a49bd6-6286-469d-b3da-62b944914a03") }
        };

            var mockBeneficiaryRepository = new Mock<IBenificiaryRepository>();
            mockBeneficiaryRepository.Setup(repo => repo.GetAllBeneficiariesAsync(userId))
                .ReturnsAsync(beneficiaries);

            var transactions = new List<UserTransaction>
        {
            new UserTransaction { TransactionId = new Guid("deab8f41-dd1f-45c3-8ca4-d49603dd4ddc"), TopUpOption = new TopUpOption { OptionName = "AED 100" } },
            new UserTransaction { TransactionId = new Guid("ba57625f-b62b-4d32-93d6-674fef7f56e4"), TopUpOption = new TopUpOption { OptionName = "AED 50" } }
        };

            var mockTransactionRepository = new Mock<ITransactionRepository>();
            mockTransactionRepository.Setup(repo => repo.GetAllTransactionAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(transactions);

            var transactionService = new TransactionService(
                mockTransactionRepository.Object,
                null,
                mockBeneficiaryRepository.Object
            );

            await Assert.ThrowsExceptionAsync<Exception>(() => transactionService.TopUpTransaction(userId, transactions[1]));
        }

        [TestMethod]
        public async Task TopUpTransaction_SufficientBalance_NoException()
        {
            var userId = Guid.NewGuid();
            var beneficiaries = new List<Beneficiary>
        {
            new Beneficiary { BenificiaryId = new Guid("2d3f71e1-5b36-4dfe-81c0-7cdba2c9e87e") },
            new Beneficiary { BenificiaryId = new Guid("33a49bd6-6286-469d-b3da-62b944914a03") }
        };

            var mockBeneficiaryRepository = new Mock<IBenificiaryRepository>();
            mockBeneficiaryRepository.Setup(repo => repo.GetAllBeneficiariesAsync(userId))
                .ReturnsAsync(beneficiaries);

            var transactions = new List<UserTransaction>
        {
            new UserTransaction { TransactionId = new Guid("deab8f41-dd1f-45c3-8ca4-d49603dd4ddc"), TopUpOption = new TopUpOption { OptionName = "AED 100" } },
            new UserTransaction { TransactionId = new Guid("ba57625f-b62b-4d32-93d6-674fef7f56e4"), TopUpOption = new TopUpOption { OptionName = "AED 50" } }
        };

            var mockTransactionRepository = new Mock<ITransactionRepository>();
            mockTransactionRepository.Setup(repo => repo.GetAllTransactionAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(transactions);

            var transactionService = new TransactionService(
               mockTransactionRepository.Object,
               null,
               mockBeneficiaryRepository.Object
           );
            var newTransaction = new UserTransaction { TopUpOption = new TopUpOption { Amount = 2000 } };

            await transactionService.TopUpTransaction(userId, newTransaction);

        }

        [TestMethod]
        public async Task TopUpTransaction_InsufficientBalance_ThrowsException()
        {
            var userId = Guid.NewGuid();
            var beneficiaries = new List<Beneficiary>
        {
            new Beneficiary { BenificiaryId = new Guid("2d3f71e1-5b36-4dfe-81c0-7cdba2c9e87e") },
            new Beneficiary { BenificiaryId = new Guid("33a49bd6-6286-469d-b3da-62b944914a03") }
        };

            var mockBeneficiaryRepository = new Mock<IBenificiaryRepository>();
            mockBeneficiaryRepository.Setup(repo => repo.GetAllBeneficiariesAsync(userId))
                .ReturnsAsync(beneficiaries);

            var transactions = new List<UserTransaction>
        {
            new UserTransaction { TransactionId = new Guid("deab8f41-dd1f-45c3-8ca4-d49603dd4ddc"), TopUpOption = new TopUpOption { OptionName = "AED 100" } },
            new UserTransaction { TransactionId = new Guid("ba57625f-b62b-4d32-93d6-674fef7f56e4"), TopUpOption = new TopUpOption { OptionName = "AED 50" } }
        };

            var mockTransactionRepository = new Mock<ITransactionRepository>();
            mockTransactionRepository.Setup(repo => repo.GetAllTransactionAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(transactions);

            var transactionService = new TransactionService(
               mockTransactionRepository.Object,
               null,
               mockBeneficiaryRepository.Object
           );

            var newTransaction = new UserTransaction { TopUpOption = new TopUpOption { Amount = 4000 } };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => transactionService.TopUpTransaction(userId, newTransaction));
        }
    }
}