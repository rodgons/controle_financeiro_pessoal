using System.Net.Mail;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Backend.Features.Emails.SendNegativeBalanceEmail;

public interface ISmtpClientFactory
{
    SmtpClient CreateClient(string server, int port, string username, string password);
}

public class SmtpClientFactory : ISmtpClientFactory
{
    public SmtpClient CreateClient(string server, int port, string username, string password)
    {
        return new SmtpClient(server, port)
        {
            EnableSsl = true,
            Credentials = new System.Net.NetworkCredential(username, password)
        };
    }
}

public class SendNegativeBalanceEmailHandler(
    IConfiguration configuration,
    ISmtpClientFactory smtpClientFactory,
    ILogger<SendNegativeBalanceEmailHandler> logger)
    : IRequestHandler<SendNegativeBalanceEmailCommand, Unit>
{
    private readonly string _smtpServer = configuration["Email:SmtpServer"] ?? throw new ArgumentNullException("Email:SmtpServer configuration is missing");
    private readonly int _smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? throw new ArgumentNullException("Email:SmtpPort configuration is missing"));
    private readonly string _smtpUsername = configuration["Email:Username"] ?? throw new ArgumentNullException("Email:Username configuration is missing");
    private readonly string _smtpPassword = configuration["Email:Password"] ?? throw new ArgumentNullException("Email:Password configuration is missing");
    private readonly string _fromEmail = configuration["Email:From"] ?? throw new ArgumentNullException("Email:From configuration is missing");
    private readonly string _toEmail = configuration["Email:To"] ?? throw new ArgumentNullException("Email:To configuration is missing");

    public async Task<Unit> Handle(SendNegativeBalanceEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var client = smtpClientFactory.CreateClient(_smtpServer, _smtpPort, _smtpUsername, _smtpPassword);

            var message = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = "Negative Balance Alert",
                Body = $"Your cash flow balance is negative ({request.Balance:C2}) as of {request.Date:dd/MM/yyyy}.",
                IsBodyHtml = true
            };

            message.To.Add(_toEmail);

            if (configuration["ASPNETCORE_ENVIRONMENT"] == "Production")
            {
                await client.SendMailAsync(message, cancellationToken);
            }
            
            logger.LogInformation("Email sent successfully to {ToEmail}", _toEmail);
            return Unit.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {ToEmail}", _toEmail);
            throw;
        }
    }
} 