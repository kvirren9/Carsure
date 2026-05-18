namespace Carsure.Services;

public interface IEmailService
{
    Task SendContactEmailAsync(
        string toEmail,
        string toName,
        string fromEmail,
        string fromName,
        string subject,
        string message,
        string adTitle,
        int adId);
}
