namespace Core.Models
{
    public record Customer(Guid Id, string Name, string? Email = null);
}
