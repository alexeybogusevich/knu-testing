using Core.Data;
using Core.Models;
using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task CreateAsync(Order? order)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetAsync(Guid customerId)
        {
            return await context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
        }
    }
}
