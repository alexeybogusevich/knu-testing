using Core.EndpointDefinitions;
using Core.Models;
using Core.Services.Interfaces;
using Core.UnitTests.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.UnitTests.EndpointDefinitons
{
    [TestClass]
    public class CustomerEndpointDefinitionTests
    {
        [TestMethod]
        public async Task GetAsync_ReturnsAllCustomersWithOKStatusCode()
        {
            // Arrange
            const int ExpectedStatusCode = StatusCodes.Status200OK;
            var expecetedCustomers = new List<Customer>
            {
                new(Guid.NewGuid(), "Customer A"),
                new(Guid.NewGuid(), "Customer B"),
            };

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(m => m.GetAsync()).ReturnsAsync(expecetedCustomers);

            var definiton = new CustomerEndpointDefinition();

            // Act
            var result = await definiton.GetAsync(mockCustomerRepository.Object);
            var actualCustomers = result.GetOkObjectResultValue<IEnumerable<Customer?>>();
            var actualStatusCode = result.GetOkObjectResultStatusCode();

            // Assert
            actualCustomers.Should().BeEquivalentTo(expecetedCustomers);
            actualStatusCode.Should().Be(ExpectedStatusCode);

            mockCustomerRepository.Verify(m => m.GetAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenCustomerExists_ReturnsRequestedCustomerWithOKStatusCode()
        {
            // Arrange
            const int ExpectedStatusCode = StatusCodes.Status200OK;
            var expectedCustomer = new Customer(Guid.NewGuid(), "Customer A");

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(m => m.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expectedCustomer);

            var definiton = new CustomerEndpointDefinition();

            // Act
            var result = await definiton.GetByIdAsync(mockCustomerRepository.Object, expectedCustomer.Id);
            var actualCustomer = result.GetOkObjectResultValue<Customer?>();
            var actualStatusCode = result.GetOkObjectResultStatusCode();

            // Assert
            actualCustomer.Should().BeEquivalentTo(expectedCustomer);
            actualStatusCode.Should().Be(ExpectedStatusCode);

            mockCustomerRepository.Verify(m => m.GetAsync(expectedCustomer.Id), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenCustomerDoesNotExist_ReturnsNullWithNotFoundStatusCode()
        {
            // Arrange
            const int ExpectedStatusCode = StatusCodes.Status404NotFound;

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(m => m.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Customer?>(null));

            var definiton = new CustomerEndpointDefinition();

            // Act
            var result = await definiton.GetByIdAsync(mockCustomerRepository.Object, Guid.NewGuid());
            var actualCustomer = result.GetOkObjectResultValue<Customer?>();
            var actualStatusCode = result.GetNotFoundResultStatusCode();

            // Assert
            actualCustomer.Should().BeNull();
            actualStatusCode.Should().Be(ExpectedStatusCode);
        }
    }
}
