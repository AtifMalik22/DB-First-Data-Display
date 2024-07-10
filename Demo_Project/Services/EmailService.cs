using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using System.Net.Mail;
namespace Demo_Project.Services
{
    public class EmailService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SendEmailWithPdfAsync(string toEmail, string subject, string body, string pdfContent)
        {
            try
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
                    IsBodyHtml = true,
                    Body = body // Use the HTML body with embedded image
                };

                mailMessage.To.Add(toEmail);

                // Generate PDF and attach it
                var pdfStream = GeneratePdf(pdfContent);
                mailMessage.Attachments.Add(new Attachment(pdfStream, "PayrollDetails.pdf", "application/pdf"));

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw; // Rethrow the exception to propagate it further
            }
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

            // Add image to PDF
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "solo.png");
            if (File.Exists(imagePath))
            {
                Image img = Image.GetInstance(imagePath);
                img.ScaleToFit(200f, 200f); // Adjust the size of the image if needed
                img.SetAbsolutePosition(25, document.PageSize.Height - img.ScaledHeight - 30); // Set position (top-left corner)
                document.Add(img);
            }
            var imagePath2 = Path.Combine(_webHostEnvironment.WebRootPath, "sign-and-stamp.png");
            if (File.Exists(imagePath2))
            {
                Image img = Image.GetInstance(imagePath2);
                img.ScaleToFit(100f, 100f); // Adjust the size of the image if needed

                // Calculate the position for the bottom-left corner
                float xPosition = 50; // Left margin
                float yPosition = 230; // Bottom margin

                // Set the absolute position of the image
                img.SetAbsolutePosition(xPosition, yPosition);

                document.Add(img);
            }

            document.Close();
            stream.Position = 0;

            return stream;
        }


    }
}
