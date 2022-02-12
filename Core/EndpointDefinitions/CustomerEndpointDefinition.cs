﻿using Core.Models;
using Core.Services;
using Core.Services.Interfaces;

namespace Core.EndpointDefinitions
{
    public class CustomerEndpointDefinition : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            app.MapGet("/customers", GetAsync);
            app.MapGet("/customers/{id}", GetByIdAsync);
            app.MapPost("/customers", CreateAsync);
            app.MapPut("/customers/{id}", UpdateAsync);
            app.MapDelete("/customers/{id}", DeleteAsync);
        }

        internal async Task<IResult> GetAsync(ICustomerRepository repository)
        {
            var customers = await repository.GetAsync();
            return Results.Ok(customers);
        }

        internal async Task<IResult> GetByIdAsync(ICustomerRepository repository, Guid id)
        {
            var customer = await repository.GetAsync(id);
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        }

        internal async Task<IResult> CreateAsync(ICustomerRepository repository, Customer customer)
        {
            await repository.CreateAsync(customer);
            return Results.Created($"/customers/{customer.Id}", customer);
        }

        internal IResult UpdateAsync(ICustomerRepository repository, Guid id, Customer updatedCustomer)
        {
            var customer = repository.GetAsync(id);
            if (customer is null)
            {
                return Results.NotFound();
            }

            repository.UpdateAsync(updatedCustomer);
            return Results.Ok(updatedCustomer);
        }

        internal async Task<IResult> DeleteAsync(ICustomerRepository repository, Guid id)
        {
            await repository.DeleteAsync(id);
            return Results.NoContent();
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
