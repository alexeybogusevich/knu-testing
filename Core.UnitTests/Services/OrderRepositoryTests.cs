using Core.Data;
using Core.Models;
using Core.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Core.UnitTests.Services
{
    [TestClass]
    public class OrderRepositoryTests
    {
        [TestMethod]
        public async Task GetAsync_WhenCustomerHasOrder_ReturnsCustomerOrdersWithThisOrder()
        {
            // Arrange
            var customer = new Customer(Guid.NewGuid(), "Customer A with some orders");
            var expectedOrder = new Order(Guid.NewGuid(), "International Order", customer.Id);

            using var context = new ApplicationDbContext(DbContextUtilities.GetContextOptions());

            await context.Customers.AddAsync(customer);
            await context.Orders.AddAsync(expectedOrder);
            await context.SaveChangesAsync();

            var repository = new OrderRepository(context);

            // Act
            var actualCustomerOrders = await repository.GetAsync(customer.Id);

            // Assert
            actualCustomerOrders.Should().Contain(expectedOrder);
        }
    }
}
