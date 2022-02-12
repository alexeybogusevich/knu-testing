using Core.Models;
using Core.Services;
using Core.Services.Interfaces;

namespace Core.EndpointDefinitions
{
    public class CustomerEndpointDefinition : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            app.MapGet("/customers", GetAllCustomers);
            app.MapGet("/customers/{id}", GetCustomerById);
            app.MapPost("/customers", CreateCustomer);
            app.MapPut("/customers/{id}", UpdateCustomer);
            app.MapDelete("/customers/{id}", DeleteCustomerById);
        }

        internal async Task<IEnumerable<Customer>> GetAllCustomers(ICustomerRepository repository)
        {
            return await repository.GetAsync();
        }

        internal async Task<IResult> GetCustomerById(ICustomerRepository repository, Guid id)
        {
            var customer = await repository.GetAsync(id);
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        }

        internal async Task<IResult> CreateCustomer(ICustomerRepository repository, Customer customer)
        {
            await repository.CreateAsync(customer);
            return Results.Created($"/customers/{customer.Id}", customer);
        }

        internal IResult UpdateCustomer(ICustomerRepository repository, Guid id, Customer updatedCustomer)
        {
            var customer = repository.GetAsync(id);
            if (customer is null)
            {
                return Results.NotFound();
            }

            repository.UpdateAsync(updatedCustomer);
            return Results.Ok(updatedCustomer);
        }

        internal IResult DeleteCustomerById(ICustomerRepository repository, Guid id)
        {
            repository.DeleteAsync(id);
            return Results.Ok();
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
        }
    }
}
