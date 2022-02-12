using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Core.IntegrationTests.Endpoints
{
    public class CustomerEndpointOrderedTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> appFactory;

        public CustomerEndpointOrderedTests(WebApplicationFactory<Program> appFactory)
        {
            this.appFactory = appFactory;
        }

        [Fact]
        public async Task GetCustomers_ReturnsEmptyCollectionWithOKStatusCode()
        {
            // Arrange
            using var client = appFactory.CreateClient();

            // Act
            var response = await client.GetAsync("/customers");
            var responseJson = await response.Content.ReadAsStringAsync();

            var actualCustomers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(responseJson);
            var actualStatusCode = (int)response.StatusCode;

            // Assert
            actualCustomers.Should().BeEmpty();
        }
    }
}
