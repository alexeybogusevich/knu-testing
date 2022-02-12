using Core.Data;
using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Core.IntegrationTests.Endpoints
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [CollectionDefinition("Non-Parallel Collection", DisableParallelization = true)]
    public class CustomerEndpointOrderedTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private static Guid CustomerId = Guid.NewGuid();
        private readonly WebApplicationFactory<Program> appFactory;

        public CustomerEndpointOrderedTests(WebApplicationFactory<Program> appFactory)
        {
            this.appFactory = appFactory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<ApplicationDbContext>(
                            options => options.UseInMemoryDatabase("OrderedTestsDatabase"));
                    }));
        }

        [Fact, Priority(1)]
        public async Task Get_WhenNoCustomerCreated_ReturnsEmptyCollection()
        {
            // Arrange
            using var client = appFactory.CreateClient();

            // Act
            var response = await client.GetAsync("/customers");
            var responseJson = await response.Content.ReadAsStringAsync();

            var actualCustomers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(responseJson);

            // Assert
            actualCustomers.Should().BeEmpty();
        }

        [Fact, Priority(2)]
        public async Task Post_WhenCustomerSupplied_ReturnsCreatedCustomer()
        {
            // Arrange
            var expectedCustomer = new Customer(CustomerId, "Ordered Customer A");
            var requestJson = JsonConvert.SerializeObject(expectedCustomer);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            using var client = appFactory.CreateClient();

            // Act
            var response = await client.PostAsync("/customers", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            var actualCustomer = JsonConvert.DeserializeObject<Customer>(responseJson);

            // Assert
            actualCustomer.Should().Be(expectedCustomer);
        }

        [Fact, Priority(3)]
        public async Task Get_WhenCustomerCreated_ReturnsNotEmptyCollection()
        {
            // Arrange
            using var client = appFactory.CreateClient();

            // Act
            var response = await client.GetAsync("/customers");
            var responseJson = await response.Content.ReadAsStringAsync();

            var actualCustomers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(responseJson);

            // Assert
            actualCustomers.Should().NotBeEmpty();
        }

        [Fact, Priority(4)]
        public async Task GetById_WhenCustomerCreated_ReturnsCreatedCustomer()
        {
            // Arrange
            using var client = appFactory.CreateClient();

            // Act
            var response = await client.GetAsync($"/customers/{CustomerId}");
            var responseJson = await response.Content.ReadAsStringAsync();

            var actualCustomer = JsonConvert.DeserializeObject<Customer>(responseJson);

            // Assert
            actualCustomer.Should().NotBeNull();
            actualCustomer.Id.Should().Be(CustomerId);
        }

        [Fact, Priority(5)]
        public async Task Delete_WhenCustomerCreated_ReturnsNoContentStatusCode()
        {
            // Arrange
            const int ExpectedStatusCode = StatusCodes.Status204NoContent;
            using var client = appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"/customers/{CustomerId}");
            var actualStatusCode = (int)response.StatusCode;

            // Assert
            actualStatusCode.Should().Be(ExpectedStatusCode);
        }

        [Fact, Priority(6)]
        public async Task GetById_WhenCustomerDeleted_ReturnsNullWithNotFoundStatusCode()
        {
            // Arrange
            const int ExpectedStatusCode = StatusCodes.Status404NotFound;
            using var client = appFactory.CreateClient();

            // Act
            var response = await client.GetAsync($"/customers/{CustomerId}");
            var responseJson = await response.Content.ReadAsStringAsync();

            var actualCustomer = JsonConvert.DeserializeObject<Customer>(responseJson);
            var actualStatusCode = (int)response.StatusCode;

            // Assert
            actualCustomer.Should().BeNull();
            actualStatusCode.Should().Be(ExpectedStatusCode);
        }

        public void Dispose()
        {
            appFactory.Dispose();
        }
    }
}
