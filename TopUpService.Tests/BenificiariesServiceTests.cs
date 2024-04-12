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
    public class BeneficiariesServiceTests
    {
        [TestMethod]
        public async Task GetAllBeneficiariesAsync_ReturnsBeneficiaries()
        {
            var mockRepository = new Mock<IBenificiaryRepository>();
            var service = new BeneficiariesService(mockRepository.Object);
            var userId = Guid.NewGuid();
            var expectedBeneficiaries = new List<Beneficiary> { new Beneficiary { NickName = "JUSTIN", PhoneNumber = "123344566", Status = Status.InActive },
                new Beneficiary {NickName = "MIKE", PhoneNumber = "98765433212", Status = Status.Active }
           };

            mockRepository.Setup(repo => repo.GetAllBeneficiariesAsync(userId))
                .ReturnsAsync(expectedBeneficiaries);

            // Act
            var actualBeneficiaries = await service.GetAllBeneficiariesAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedBeneficiaries.ToList(), actualBeneficiaries.ToList());
        }

        [TestMethod]
        public async Task GetActiveBeneficiariesAsync_ReturnsActiveBeneficiaries()
        {
            var mockRepository = new Mock<IBenificiaryRepository>();
            var service = new BeneficiariesService(mockRepository.Object);
            var userId = Guid.NewGuid();
            var expectedBeneficiaries = new List<Beneficiary> { new Beneficiary { NickName = "MIKE", PhoneNumber = "98765433212", Status = Status.Active } };

            mockRepository.Setup(repo => repo.GetActiveBeneficiariesAsync(userId))
                .ReturnsAsync(expectedBeneficiaries);

            // Act
            var actualBeneficiaries = await service.GetActiveBeneficiariesAsync(userId);

            // Assert
            CollectionAssert.AreEqual(expectedBeneficiaries.ToList(), actualBeneficiaries.ToList());
        }

        [TestMethod]
        public async Task GetBeneficiaryByIdAsync_ReturnsBeneficiary()
        {
            var mockRepository = new Mock<IBenificiaryRepository>();
            var service = new BeneficiariesService(mockRepository.Object);
            var beneficiaryId = Guid.NewGuid();
            var expectedBeneficiary = new Beneficiary { BenificiaryId = beneficiaryId, NickName = "MIKE", PhoneNumber = "98765433212", Status = Status.Active };

            mockRepository.Setup(repo => repo.GetBeneficiaryByIdAsync(beneficiaryId))
                .ReturnsAsync(expectedBeneficiary);

            // Act
            var actualBeneficiary = await service.GetBeneficiaryByIdAsync(beneficiaryId);

            // Assert
            Assert.AreEqual(expectedBeneficiary, actualBeneficiary);
        }

        [TestMethod]
        public async Task DeleteBeneficiaryAsync_ReturnsTrue()
        {
            // Arrange
            var mockRepository = new Mock<IBenificiaryRepository>();
            var service = new BeneficiariesService(mockRepository.Object);
            var beneficiaryId = Guid.NewGuid();

            mockRepository.Setup(repo => repo.DeleteBeneficiaryAsync(beneficiaryId))
                .ReturnsAsync(true);

            // Act
            var result = await service.DeleteBeneficiaryAsync(beneficiaryId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CreateBeneficiaryAsync_LimitExceeded_ThrowsException()
        {
            var mockRepository = new Mock<IBenificiaryRepository>();
            var service = new BeneficiariesService(mockRepository.Object);
            var userId = Guid.NewGuid();
            var beneficiaryId = Guid.NewGuid();
            var newBeneficiary = new Beneficiary { UserId = userId, BenificiaryId = beneficiaryId };
            var existingBeneficiaries = new List<Beneficiary> { new Beneficiary { BenificiaryId = beneficiaryId, NickName = "MIKE", PhoneNumber = "98765433212", Status = Status.Active }      };

            mockRepository.Setup(repo => repo.GetActiveBeneficiariesAsync(userId))
                .ReturnsAsync(existingBeneficiaries);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => service.CreateBeneficiaryAsync(newBeneficiary));
        }


    }
}