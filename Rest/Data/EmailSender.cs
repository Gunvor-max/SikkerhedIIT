using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MimeKit;

namespace Rest.Data
{
    public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_configuration["FromName"], _configuration["FromEmail"]));
        emailMessage.To.Add(new MailboxAddress(string.Empty, email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = htmlMessage };

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_configuration["SmtpServer"], int.Parse(_configuration["SmtpPort"]), MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["SmtpUsername"], _configuration["SmtpPassword"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
                _logger.LogInformation($"Email to {email} queued successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to {email}: {ex.Message}");
                throw;
            }
        }
    }
}
}
