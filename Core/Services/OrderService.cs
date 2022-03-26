using Core.Models;
using Core.Services.Interfaces;

namespace Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IEmailSender emailSender;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IEmailSender emailSender)
        {
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
            this.emailSender = emailSender;
        }

        public async Task CreateAsync(Order order)
        {
            var customer = await customerRepository.GetAsync(order.CustomerId);
            if (customer == null)
            {
                throw new ArgumentException($"Customer not found. Id: {order.CustomerId}");
            }

            await orderRepository.CreateAsync(order);
            await emailSender.SendAsync(customer.Email, $"Your order [{order.Description}] has been placed.");
        }
    }
}
