using Core.Data;
using Core.Models;
using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext context;

        public CustomerRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Customer?> GetAsync(Guid id)
        {
            return await context.Customers.FindAsync(id);
        }

        public async Task<IEnumerable<Customer?>> GetAsync()
        {
            return await context.Customers.ToListAsync();
        }

        public async Task CreateAsync(Customer? customer)
        {
            if (customer == null)
            {
                return;
            }

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer? customer)
        {
            if (customer == null)
            {
                return;
            }

            context.Customers.Update(customer);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer == null)
            {
                return;
            }

            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
        }
    }
}
