namespace Core.Models
{
    public record OrderItem(Guid Id, string ProductName, double UnitPrice, int Units)
    {
        public double Amount => Units * UnitPrice * (1 - Discount);

        public double Discount => Units switch
        {
            >= 2 and <= 5 => .25,
            > 5 => .5,
            _ => 0,
        };
    }
}
