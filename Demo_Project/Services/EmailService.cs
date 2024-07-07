using System.Net.Mail;
using System.Net;
using System.Reflection.Metadata;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Document = iTextSharp.text.Document;

namespace Demo_Project.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailWithPdfAsync(string toEmail, string subject, string body, string pdfContent)
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

            // Generate PDF and attach it
            var pdfStream = GeneratePdf(pdfContent);
            mailMessage.Attachments.Add(new Attachment(pdfStream, "PayrollDetails.pdf", "application/pdf"));

            await smtpClient.SendMailAsync(mailMessage);
        }

        private MemoryStream GeneratePdf(string pdfContent)
        {
            var document = new Document();
            var stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream).CloseStream = false;

            document.Open();
            document.Add(new Paragraph(pdfContent));
            document.Close();
            stream.Position = 0;

            return stream;
        }
    }
}
