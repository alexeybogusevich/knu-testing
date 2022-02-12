using Core.Models;

namespace Core.Services.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetAsync(Guid id);

        Task<IEnumerable<Customer?>> GetAsync();

        Task CreateAsync(Customer? customer);

        Task UpdateAsync(Customer? customer);

        Task DeleteAsync(Guid id);
    }
}
