using Moq;
using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using MySapProject.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MySapProject.Application.Tests.Services
{
    public class BusinessPartnerServiceTests
    {
        private readonly Mock<IBusinessPartnerRepository> _mockRepo;
        private readonly Mock<ILogger<BusinessPartnerService>> _mockLogger;
        private readonly BusinessPartnerService _service;

        public BusinessPartnerServiceTests()
        {
            _mockRepo = new Mock<IBusinessPartnerRepository>();
            _mockLogger = new Mock<ILogger<BusinessPartnerService>>();
            _service = new BusinessPartnerService(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetBusinessPartnerByIdAsync_WithValidId_ReturnsBusinessPartner()
        {
            // Arrange
            var bpId = "BP123";
            var expectedDto = new BusinessPartnerDto { CardCode = bpId, CardName = "Test BP" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(bpId)).ReturnsAsync(expectedDto);

            // Act
            var result = await _service.GetBusinessPartnerByIdAsync(bpId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bpId, result.CardCode);
            _mockRepo.Verify(repo => repo.GetByIdAsync(bpId), Times.Once);
        }

        [Fact]
        public async Task GetBusinessPartnerByIdAsync_WithEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var bpId = "";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetBusinessPartnerByIdAsync(bpId));
            _mockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetBusinessPartnerByIdAsync_WhenRepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            var bpId = "NonExistentBP";
            _mockRepo.Setup(repo => repo.GetByIdAsync(bpId)).ReturnsAsync((BusinessPartnerDto)null);

            // Act
            var result = await _service.GetBusinessPartnerByIdAsync(bpId);

            // Assert
            Assert.Null(result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(bpId), Times.Once);
        }

        [Fact]
        public async Task GetAllBusinessPartnersAsync_ReturnsListOfBusinessPartners()
        {
            // Arrange
            var expectedList = new List<BusinessPartnerDto>
            {
                new BusinessPartnerDto { CardCode = "BP1", CardName = "BP One" },
                new BusinessPartnerDto { CardCode = "BP2", CardName = "BP Two" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedList);

            // Act
            var result = await _service.GetAllBusinessPartnersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateBusinessPartnerAsync_WithValidDto_ReturnsCreatedDto()
        {
            // Arrange
            var inputDto = new BusinessPartnerDto { CardName = "New BP", Phone1 = "12345", Address = "Test Address" };
            var expectedDto = new BusinessPartnerDto { CardCode = "GenBP001", CardName = "New BP", Phone1 = "12345", Address = "Test Address" };
            _mockRepo.Setup(repo => repo.CreateAsync(inputDto)).ReturnsAsync(expectedDto);

            // Act
            var result = await _service.CreateBusinessPartnerAsync(inputDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.CardCode, result.CardCode);
            Assert.Equal(inputDto.CardName, result.CardName);
            _mockRepo.Verify(repo => repo.CreateAsync(inputDto), Times.Once);
        }

        [Fact]
        public async Task CreateBusinessPartnerAsync_WithNullDto_ThrowsArgumentNullException()
        {
            // Arrange
            BusinessPartnerDto inputDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateBusinessPartnerAsync(inputDto));
             _mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<BusinessPartnerDto>()), Times.Never);
        }

        [Fact]
        public async Task CreateBusinessPartnerAsync_WithEmptyCardName_ThrowsArgumentException()
        {
            // Arrange
            var inputDto = new BusinessPartnerDto { CardName = "" }; // Invalid

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateBusinessPartnerAsync(inputDto));
            Assert.Equal("businessPartnerDto.CardName", ex.ParamName);
            _mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<BusinessPartnerDto>()), Times.Never);
        }


        [Fact]
        public async Task UpdateBusinessPartnerAsync_WithValidIdAndDto_ReturnsUpdatedDto()
        {
            // Arrange
            var bpId = "BP123";
            var inputDto = new BusinessPartnerDto { CardCode = bpId, CardName = "Updated BP" }; // CardCode in DTO might be ignored or validated by service
            var expectedDto = new BusinessPartnerDto { CardCode = bpId, CardName = "Updated BP" };
            _mockRepo.Setup(repo => repo.UpdateAsync(bpId, inputDto)).ReturnsAsync(expectedDto);

            // Act
            var result = await _service.UpdateBusinessPartnerAsync(bpId, inputDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.CardName, result.CardName);
            _mockRepo.Verify(repo => repo.UpdateAsync(bpId, inputDto), Times.Once);
        }

        [Fact]
        public async Task UpdateBusinessPartnerAsync_WithEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var bpId = "";
            var inputDto = new BusinessPartnerDto { CardName = "Valid Name" };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateBusinessPartnerAsync(bpId, inputDto));
            Assert.Equal("id", ex.ParamName);
            _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<BusinessPartnerDto>()), Times.Never);
        }

        [Fact]
        public async Task UpdateBusinessPartnerAsync_WithNullDto_ThrowsArgumentNullException()
        {
            // Arrange
            var bpId = "BP123";
            BusinessPartnerDto inputDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateBusinessPartnerAsync(bpId, inputDto));
            _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<BusinessPartnerDto>()), Times.Never);
        }

        [Fact]
        public async Task UpdateBusinessPartnerAsync_WithEmptyCardName_ThrowsArgumentException()
        {
            // Arrange
            var bpId = "BP123";
            var inputDto = new BusinessPartnerDto { CardName = "" }; // Invalid

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateBusinessPartnerAsync(bpId, inputDto));
            Assert.Equal("businessPartnerDto.CardName", ex.ParamName);
            _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<BusinessPartnerDto>()), Times.Never);
        }

        [Fact]
        public async Task DeleteBusinessPartnerAsync_WithValidId_CallsRepositoryDelete()
        {
            // Arrange
            var bpId = "BP123";
            _mockRepo.Setup(repo => repo.DeleteAsync(bpId)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteBusinessPartnerAsync(bpId);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteAsync(bpId), Times.Once);
        }

        [Fact]
        public async Task DeleteBusinessPartnerAsync_WithEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var bpId = "";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteBusinessPartnerAsync(bpId));
            _mockRepo.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
