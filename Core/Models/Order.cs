namespace Core.Models
{
    public class Order
    {
        private readonly List<OrderItem> orderItems;

        public Order(Guid id, string description, Guid customerId)
        {
            Id = id;
            Description = description;
            CustomerId = customerId;
            orderItems = new List<OrderItem>();
        }

        public Guid Id { get; set; }

        public string Description { get; set; }

        public Guid CustomerId { get; set; }

        public IReadOnlyCollection<OrderItem> OrderItems => orderItems;

        public double Amount => orderItems.Sum(item => item.Amount);


        public void AddOrderItem(string productName, double unitPrice, int units = 1)
        {
            var oderItem = new OrderItem(Guid.NewGuid(), productName, unitPrice, units);
            orderItems.Add(oderItem);
        }
    }
}
