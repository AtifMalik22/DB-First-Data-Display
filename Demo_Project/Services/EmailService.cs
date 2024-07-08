using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Net;
using System.Net.Mail;
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


        private MemoryStream GeneratePdf(string htmlContent)
        {
            var stream = new MemoryStream();
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;

            document.Open();
            using (var stringReader = new StringReader(htmlContent))
            {
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
            }
            document.Close();
            stream.Position = 0;

            return stream;
        }


    }
}
