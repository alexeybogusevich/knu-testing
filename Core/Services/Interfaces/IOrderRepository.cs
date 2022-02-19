using Core.Models;

namespace Core.Services.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAsync(Guid customerId);

        Task CreateAsync(Order? order);
    }
}
