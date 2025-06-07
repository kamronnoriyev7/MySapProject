using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using MySapProject.SAP.WebApi.Controllers; // Make sure this using is correct
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MySapProject.SAP.WebApi.Tests.Controllers
{
    public class BusinessPartnersControllerTests
    {
        private readonly Mock<IBusinessPartnerService> _mockService;
        private readonly Mock<ILogger<BusinessPartnersController>> _mockLogger;
        private readonly BusinessPartnersController _controller;

        public BusinessPartnersControllerTests()
        {
            _mockService = new Mock<IBusinessPartnerService>();
            _mockLogger = new Mock<ILogger<BusinessPartnersController>>();
            _controller = new BusinessPartnersController(_mockService.Object, _mockLogger.Object);
        }

        // Test Get All
        [Fact]
        public async Task GetAllBusinessPartners_ReturnsOkObjectResult_WithListOfBps()
        {
            // Arrange
            var bps = new List<BusinessPartnerDto>
            {
                new BusinessPartnerDto { CardCode = "1", CardName = "BP1" },
                new BusinessPartnerDto { CardCode = "2", CardName = "BP2" }
            };
            _mockService.Setup(s => s.GetAllBusinessPartnersAsync()).ReturnsAsync(bps);

            // Act
            var result = await _controller.GetAllBusinessPartners();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<BusinessPartnerDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetAllBusinessPartners_ServiceThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllBusinessPartnersAsync()).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.GetAllBusinessPartners();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        // Test Get By ID
        [Fact]
        public async Task GetBusinessPartnerById_WithValidId_ReturnsOkObjectResult_WithBp()
        {
            // Arrange
            var bpId = "123";
            var bpDto = new BusinessPartnerDto { CardCode = bpId, CardName = "Test BP" };
            _mockService.Setup(s => s.GetBusinessPartnerByIdAsync(bpId)).ReturnsAsync(bpDto);

            // Act
            var result = await _controller.GetBusinessPartnerById(bpId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<BusinessPartnerDto>(okResult.Value);
            Assert.Equal(bpId, returnValue.CardCode);
        }

        [Fact]
        public async Task GetBusinessPartnerById_WithEmptyId_ReturnsBadRequest()
        {
            // Arrange
            // Act
            var result = await _controller.GetBusinessPartnerById("");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetBusinessPartnerById_NotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var bpId = "unknown";
            _mockService.Setup(s => s.GetBusinessPartnerByIdAsync(bpId)).ReturnsAsync((BusinessPartnerDto)null);

            // Act
            var result = await _controller.GetBusinessPartnerById(bpId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetBusinessPartnerById_ServiceThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var bpId = "validIdButCausesArgEx";
            _mockService.Setup(s => s.GetBusinessPartnerByIdAsync(bpId)).ThrowsAsync(new ArgumentException("Test ArgumentException"));

            // Act
            var result = await _controller.GetBusinessPartnerById(bpId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetBusinessPartnerById_ServiceThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var bpId = "errorId";
            _mockService.Setup(s => s.GetBusinessPartnerByIdAsync(bpId)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.GetBusinessPartnerById(bpId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }


        // Test Create
        [Fact]
        public async Task CreateBusinessPartner_WithValidDto_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var inputDto = new BusinessPartnerDto { CardName = "New BP" };
            var resultDto = new BusinessPartnerDto { CardCode = "Gen001", CardName = "New BP" };
            _mockService.Setup(s => s.CreateBusinessPartnerAsync(inputDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _controller.CreateBusinessPartner(inputDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetBusinessPartnerById), createdAtActionResult.ActionName);
            Assert.Equal(resultDto.CardCode, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(resultDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task CreateBusinessPartner_WithNullDto_ReturnsBadRequest()
        {
            // Arrange
            // Act
            var result = await _controller.CreateBusinessPartner(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateBusinessPartner_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var inputDto = new BusinessPartnerDto { CardName = "New BP" };
            _controller.ModelState.AddModelError("Error", "Sample model error");

            // Act
            var result = await _controller.CreateBusinessPartner(inputDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateBusinessPartner_ServiceThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var inputDto = new BusinessPartnerDto { CardName = "Causes ArgEx" };
            _mockService.Setup(s => s.CreateBusinessPartnerAsync(inputDto)).ThrowsAsync(new ArgumentException("Test ArgumentException"));

            // Act
            var result = await _controller.CreateBusinessPartner(inputDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateBusinessPartner_ServiceThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var inputDto = new BusinessPartnerDto { CardName = "Causes Ex" };
            _mockService.Setup(s => s.CreateBusinessPartnerAsync(inputDto)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.CreateBusinessPartner(inputDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        // Test Update
        [Fact]
        public async Task UpdateBusinessPartner_WithValidIdAndDto_ReturnsOkObjectResult()
        {
            // Arrange
            var bpId = "123";
            var inputDto = new BusinessPartnerDto { CardName = "Updated BP" };
            var resultDto = new BusinessPartnerDto { CardCode = bpId, CardName = "Updated BP" };
            _mockService.Setup(s => s.UpdateBusinessPartnerAsync(bpId, inputDto)).ReturnsAsync(resultDto);

            // Act
            var result = await _controller.UpdateBusinessPartner(bpId, inputDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(resultDto, okResult.Value);
        }

        [Fact]
        public async Task UpdateBusinessPartner_WithEmptyId_ReturnsBadRequest()
        {
            // Arrange
            var inputDto = new BusinessPartnerDto { CardName = "Updated BP" };
            // Act
            var result = await _controller.UpdateBusinessPartner("", inputDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBusinessPartner_WithNullDto_ReturnsBadRequest()
        {
            // Arrange
            var bpId = "123";
            // Act
            var result = await _controller.UpdateBusinessPartner(bpId, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBusinessPartner_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var bpId = "123";
            var inputDto = new BusinessPartnerDto { CardName = "Updated BP" };
            _controller.ModelState.AddModelError("Error", "Sample model error");

            // Act
            var result = await _controller.UpdateBusinessPartner(bpId, inputDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBusinessPartner_ServiceReturnsNull_ReturnsNotFound()
        {
            // Arrange
            var bpId = "unknown";
            var inputDto = new BusinessPartnerDto { CardName = "Updated BP" };
            _mockService.Setup(s => s.UpdateBusinessPartnerAsync(bpId, inputDto)).ReturnsAsync((BusinessPartnerDto)null);

            // Act
            var result = await _controller.UpdateBusinessPartner(bpId, inputDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBusinessPartner_ServiceThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var bpId = "123";
            var inputDto = new BusinessPartnerDto { CardName = "Causes ArgEx" };
            _mockService.Setup(s => s.UpdateBusinessPartnerAsync(bpId, inputDto)).ThrowsAsync(new ArgumentException("Test ArgumentException"));

            // Act
            var result = await _controller.UpdateBusinessPartner(bpId, inputDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBusinessPartner_ServiceThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var bpId = "123";
            var inputDto = new BusinessPartnerDto { CardName = "Causes Ex" };
            _mockService.Setup(s => s.UpdateBusinessPartnerAsync(bpId, inputDto)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.UpdateBusinessPartner(bpId, inputDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        // Test Delete
        [Fact]
        public async Task DeleteBusinessPartner_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            var bpId = "123";
            _mockService.Setup(s => s.DeleteBusinessPartnerAsync(bpId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBusinessPartner(bpId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBusinessPartner_WithEmptyId_ReturnsBadRequest()
        {
            // Arrange
            // Act
            var result = await _controller.DeleteBusinessPartner("");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteBusinessPartner_ServiceThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var bpId = "123";
            _mockService.Setup(s => s.DeleteBusinessPartnerAsync(bpId)).ThrowsAsync(new ArgumentException("Test ArgumentException"));

            // Act
            var result = await _controller.DeleteBusinessPartner(bpId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteBusinessPartner_ServiceThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var bpId = "123";
            _mockService.Setup(s => s.DeleteBusinessPartnerAsync(bpId)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.DeleteBusinessPartner(bpId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
