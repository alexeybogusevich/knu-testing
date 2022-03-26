using Core.Models;
using Core.Services;
using Core.Services.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.UnitTests.Services
{
    [TestClass]
    public class OrderServiceStateTests
    {
        [TestMethod]
        public async Task CreateAsync_WhenOrderCreated_CustomerShouldBeOnTheListOfRecipients()
        {
            // Arrange
            var customer = new Customer(Guid.NewGuid(), "Customer A", "customerA@fakeEmail.com");
            var order = new Order(Guid.NewGuid(), "International order", customer.Id);

            var fakeOrderRepository = new FakeOrderRepository();
            var fakeCustomerRepository = new FakeCustomerRepository();
            var stubEmailSender = new StubEmailSender();

            await fakeCustomerRepository.CreateAsync(customer);

            var service = new OrderService(fakeOrderRepository, fakeCustomerRepository, stubEmailSender);

            // Act
            await service.CreateAsync(order);

            // Arrange
            stubEmailSender.Recipients.Should().Contain(customer.Email);
        }
    }

    class FakeOrderRepository : IOrderRepository
    {
        public List<Order> Orders = new();

        public async Task CreateAsync(Order? order)
        {
            if (order == null)
            {
                throw new ArgumentException("Order is null.");
            }

            Orders.Add(order);
            await Task.FromResult(true);
        }

        public async Task<IEnumerable<Order>> GetAsync(Guid customerId)
        {
            await Task.FromResult(true);
            return Orders.Where(o => o.CustomerId == customerId);
        }
    }

    class FakeCustomerRepository : ICustomerRepository
    {
        public List<Customer> Customers = new();

        public async Task CreateAsync(Customer? customer)
        {
            if (customer == null)
            {
                throw new ArgumentException("Customer is null");
            }

            Customers.Add(customer);
            await Task.FromResult(true);
        }

        public async Task DeleteAsync(Guid id)
        {
            var customerToDelete = Customers.FirstOrDefault(c => c.Id == id);
            if (customerToDelete != null)
            {
                Customers.Remove(customerToDelete);
            }

            await Task.FromResult(true);
        }

        public async Task<Customer?> GetAsync(Guid id)
        {
            await Task.FromResult(true);
            return Customers.FirstOrDefault(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer?>> GetAsync()
        {
            await Task.FromResult(true);
            return Customers;
        }

        public async Task UpdateAsync(Customer? customer)
        {
            await Task.FromResult(true);

            var customerToUpdate = Customers.FirstOrDefault(c => c.Id == customer?.Id);
            if (customerToUpdate != null && customer != null)
            {
                Customers.Remove(customerToUpdate);
                Customers.Add(customer);
            }
        }
    }

    class StubEmailSender : IEmailSender
    {
        public List<string> Recipients = new();

        public async Task SendAsync(string? recipient, string? message)
        {
            if (recipient == null)
            {
                throw new ArgumentException("Recipient is null.");
            }

            Recipients.Add(recipient);
            await Task.FromResult(true);
        }
    }
}
