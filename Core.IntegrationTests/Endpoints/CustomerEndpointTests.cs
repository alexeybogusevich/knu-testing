using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.IntegrationTests.Endpoints
{
    public class CustomerEndpointTests 
    {
        [Fact]
        public async Task Post_WhenCustomerSupplied_ReturnsCreatedCustomerWithCreatedStatusCode()
        {
            // Arrange
            const int ExpectedStatusCode = StatusCodes.Status201Created;

            var expectedCustomer = new Customer(Guid.NewGuid(), "Customer A");
            var requestJson = JsonConvert.SerializeObject(expectedCustomer);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();

            // Act
            var response = await client.PostAsync("/customers", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            var actualCustomer = JsonConvert.DeserializeObject<Customer>(responseJson);
            var actualStatusCode = (int)response.StatusCode;

            // Assert
            actualCustomer.Should().Be(expectedCustomer);
            actualStatusCode.Should().Be(ExpectedStatusCode);
        }
    }
}
