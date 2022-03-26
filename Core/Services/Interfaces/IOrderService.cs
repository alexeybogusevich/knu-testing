using Core.Models;

namespace Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateAsync(Order order);
    }
}
