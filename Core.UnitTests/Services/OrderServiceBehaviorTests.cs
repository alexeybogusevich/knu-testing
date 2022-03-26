using Core.Models;
using Core.Services;
using Core.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Core.UnitTests.Services
{
    [TestClass]
    public class OrderServiceBehaviorTests
    {
        [TestMethod]
        public async Task CreateAsync_WhenOrderCreated_CustomerShouldBeNotifiedByEmail()
        {
            // Arrange
            var customer = new Customer(Guid.NewGuid(), "Customer A", "customerA@fakeEmail.com");
            var order = new Order(Guid.NewGuid(), "International order", customer.Id);

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(m => m.GetAsync(It.IsAny<Guid>())).ReturnsAsync(customer);

            var mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository.Setup(m => m.CreateAsync(It.IsAny<Order>()));

            var mockEmailSender = new Mock<IEmailSender>();
            mockEmailSender.Setup(m => m.SendAsync(It.IsAny<string>(), It.IsAny<string>()));

            var service = new OrderService(mockOrderRepository.Object, mockCustomerRepository.Object, mockEmailSender.Object);

            // Act
            await service.CreateAsync(order);

            // Arrange
            mockEmailSender.Verify(m => m.SendAsync(customer.Email, $"Your order [{order.Description}] has been placed."));
        }
    }
}
