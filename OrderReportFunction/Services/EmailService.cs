using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OrderReportFunction.Services;

public class SendGridEmailService : IEmailService
{
    private readonly string _apiKey;
    private readonly string _fromEmail;

    public SendGridEmailService(IConfiguration config)
    {
        _apiKey = config["SendGridApiKey"] ?? throw new ArgumentNullException(nameof(config), "SendGridApiKey is missing in configuration.");
        _fromEmail = config["FromEmail"] ?? throw new ArgumentNullException(nameof(config), "FromEmail is missing in configuration.");
    }

    public async Task SendReportAsync(string toEmail, byte[] pdfBytes, string fileName)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(_fromEmail, "Monthly Reports");
        var to = new EmailAddress(toEmail);
        var subject = "Your Monthly Order Report";
        var plainTextContent = "Please find your monthly report attached.";
        var htmlContent = "<strong>Please find your monthly report attached.</strong>";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        msg.AddAttachment(fileName, Convert.ToBase64String(pdfBytes), "application/pdf");

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to send email to {toEmail}. Status: {response.StatusCode}");
        }
    }
}
