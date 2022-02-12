using Core.Models;
using Core.Services.Interfaces;

namespace Core.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        public void CreateAsync(Customer? customer)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Customer? GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
