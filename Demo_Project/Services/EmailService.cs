using System.Net.Mail;
using System.Net;

namespace Demo_Project.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailConfig = _configuration.GetSection("EmailCred");
            var smtpClient = new SmtpClient(emailConfig["SmtpHost"])
            {
                Port = 587,
                Credentials = new NetworkCredential(emailConfig["UserName"], emailConfig["Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailConfig["UserName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
