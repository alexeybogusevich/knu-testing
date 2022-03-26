namespace Core.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendAsync(string? recipient, string? message);
    }
}
