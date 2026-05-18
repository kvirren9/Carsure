using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Carsure.Services;

public class EmailService : IEmailService
{
  private readonly IConfiguration _config;
  private readonly ILogger<EmailService> _logger;

  public EmailService(IConfiguration config, ILogger<EmailService> logger)
  {
    _config = config;
    _logger = logger;
  }

  public async Task SendContactEmailAsync(
      string toEmail,
      string toName,
      string fromEmail,
      string fromName,
      string subject,
      string message,
      string adTitle,
      int adId)
  {
    var smtpSettings = _config.GetSection("Smtp");
    var host = smtpSettings["Host"] ?? throw new InvalidOperationException("SMTP Host is not configured.");
    var port = int.Parse(smtpSettings["Port"] ?? "587");
    var user = smtpSettings["Username"] ?? throw new InvalidOperationException("SMTP Username is not configured.");
    var pass = smtpSettings["Password"] ?? throw new InvalidOperationException("SMTP Password is not configured.");
    var senderName = smtpSettings["SenderName"] ?? "Carsure";
    var senderEmail = smtpSettings["SenderEmail"] ?? user;

    var email = new MimeMessage();
    email.From.Add(new MailboxAddress(senderName, senderEmail));
    email.To.Add(new MailboxAddress(toName, toEmail));
    email.ReplyTo.Add(new MailboxAddress(fromName, fromEmail));
    email.Subject = $"[Carsure] {subject} – Annons: {adTitle}";

    var bodyBuilder = new BodyBuilder
    {
      HtmlBody = BuildHtmlBody(fromName, fromEmail, subject, message, adTitle, adId),
      TextBody = BuildTextBody(fromName, fromEmail, subject, message, adTitle, adId)
    };

    email.Body = bodyBuilder.ToMessageBody();

    using var smtp = new SmtpClient();
    try
    {
      await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
      await smtp.AuthenticateAsync(user, pass);
      await smtp.SendAsync(email);
      _logger.LogInformation("Contact email sent to {ToEmail} regarding ad #{AdId}", toEmail, adId);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex,
          "Failed to send contact email to {ToEmail} for ad #{AdId}. " +
          "Check SMTP settings in appsettings.json (Smtp:Host, Smtp:Username, Smtp:Password).",
          toEmail, adId);
      throw;
    }
    finally
    {
      await smtp.DisconnectAsync(true);
    }
  }

  private static string BuildHtmlBody(
      string fromName, string fromEmail,
      string subject, string message,
      string adTitle, int adId)
  {
    var encodedMessage = System.Net.WebUtility.HtmlEncode(message).Replace("\n", "<br/>");
    return $"""
            <!DOCTYPE html>
            <html lang="sv">
            <head><meta charset="utf-8"/></head>
            <body style="font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: 0 auto;">
              <div style="background:#1a1a2e; padding: 24px; border-radius: 8px 8px 0 0;">
                <h1 style="color:#e94560; margin:0; font-size:24px;">Carsure</h1>
                <p style="color:#aaa; margin:4px 0 0;">Ny förfrågan om din annons</p>
              </div>
              <div style="background:#f9f9f9; padding: 24px; border: 1px solid #ddd; border-top: none; border-radius: 0 0 8px 8px;">
                <h2 style="margin-top:0;">Du har fått ett meddelande!</h2>
                <p>En intresserad köpare har kontaktat dig angående din annons <strong>{System.Net.WebUtility.HtmlEncode(adTitle)}</strong>.</p>
                <table style="width:100%; border-collapse:collapse; margin-bottom:16px;">
                  <tr>
                    <td style="padding:8px; background:#eee; font-weight:bold; width:140px;">Från</td>
                    <td style="padding:8px; background:#fff;">{System.Net.WebUtility.HtmlEncode(fromName)} &lt;{System.Net.WebUtility.HtmlEncode(fromEmail)}&gt;</td>
                  </tr>
                  <tr>
                    <td style="padding:8px; background:#eee; font-weight:bold;">Ämne</td>
                    <td style="padding:8px; background:#fff;">{System.Net.WebUtility.HtmlEncode(subject)}</td>
                  </tr>
                </table>
                <div style="background:#fff; border:1px solid #ddd; border-radius:4px; padding:16px; margin-bottom:16px;">
                  <p style="margin:0; white-space:pre-wrap;">{encodedMessage}</p>
                </div>
                <p style="color:#666; font-size:13px;">
                  Svara direkt till köparen på: <a href="mailto:{System.Net.WebUtility.HtmlEncode(fromEmail)}">{System.Net.WebUtility.HtmlEncode(fromEmail)}</a>
                </p>
                <hr style="border:none; border-top:1px solid #ddd; margin:16px 0;"/>
                <p style="color:#999; font-size:12px; margin:0;">
                  Detta meddelande skickades via Carsure. Annons-ID: #{adId}
                </p>
              </div>
            </body>
            </html>
            """;
  }

  private static string BuildTextBody(
      string fromName, string fromEmail,
      string subject, string message,
      string adTitle, int adId)
  {
    return $"""
            Du har fått ett meddelande via Carsure!

            Annons: {adTitle} (#{adId})
            Från: {fromName} <{fromEmail}>
            Ämne: {subject}

            Meddelande:
            {message}

            ---
            Svara direkt till köparen på: {fromEmail}
            """;
  }
}
