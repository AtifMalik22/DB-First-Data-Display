using Demo_Project.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OfficeOpenXml;
using Syncfusion.Blazor.Inputs;

namespace Demo_Project.Pages
{
    public partial class Uploader
    {
        UploadFile filess = new();
        private List<UploadFile> fileContent=new();
        SfUploader sfImageUploader { get; set; }
        private IBrowserFile selectedFile;
        protected override async Task OnInitializedAsync()
        {
            fileContent = await uploadFileService.GetDataAsync();
        }
        private void HandleFileSelected(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
        }

        private async Task Add()
        {
            if (selectedFile == null)
            {
                Console.WriteLine("No file selected.");
                return;
            }

            var file = selectedFile;
            if (!file.Name.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Uploaded file is not an Excel file (.xlsx).");
                return;
            }

            var filePath = Path.Combine(Environment.WebRootPath, "UploadFiles", file.Name);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream().CopyToAsync(stream);
            }

            fileContent = ReadExcelFile(filePath);

            if (fileContent != null && fileContent.Any())
            {
                await uploadFileService.SaveFileContentToDatabase(fileContent, filess.Date);
                StateHasChanged();
            }
        }

        private List<UploadFile> ReadExcelFile(string filePath)
        {
            var lines = new List<UploadFile>();

            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet

                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++) // Skip the header row
                {
                    try
                    {
                        var uploadFile = new UploadFile
                        {
                            SrNo = Convert.ToInt32(worksheet.Cells[row, 1].Value?.ToString().Trim() ?? "0"),
                            EmoployeeId = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                            ServiceCategory = worksheet.Cells[row, 3].Value?.ToString().Trim(),
                            EmployeeName = worksheet.Cells[row, 4].Value?.ToString().Trim(),
                            Cnic = worksheet.Cells[row, 5].Value?.ToString().Trim(),
                            Email = worksheet.Cells[row, 6].Value?.ToString().Trim(),
                            JoiningDate = worksheet.Cells[row, 7].Value?.ToString().Trim(),
                            MonthDays = Convert.ToInt32(worksheet.Cells[row, 8].Value?.ToString().Trim() ?? "0"),
                            PresentDays = Convert.ToInt32(worksheet.Cells[row, 9].Value?.ToString().Trim() ?? "0"),
                            OfferedSalary = Convert.ToInt32(worksheet.Cells[row, 10].Value?.ToString().Trim() ?? "0"),
                            LeaveDeduction = Convert.ToInt32(worksheet.Cells[row, 11].Value?.ToString().Trim() ?? "0"),
                            BasicPayafterDeduction = Convert.ToInt32(worksheet.Cells[row, 12].Value?.ToString().Trim() ?? "0"),
                            Arrears = Convert.ToInt32(worksheet.Cells[row, 13].Value?.ToString().Trim() ?? "0"),
                            Allowances = Convert.ToInt32(worksheet.Cells[row, 14].Value?.ToString().Trim() ?? "0"),
                            AdvancesLoan = Convert.ToInt32(worksheet.Cells[row, 15].Value?.ToString().Trim() ?? "0"),
                            GrossSalary = Convert.ToInt32(worksheet.Cells[row, 16].Value?.ToString().Trim() ?? "0"),
                            AdvancesLoanDeduction = Convert.ToInt32(worksheet.Cells[row, 17].Value?.ToString().Trim() ?? "0"),
                            EOBI = Convert.ToInt32(worksheet.Cells[row, 18].Value?.ToString().Trim() ?? "0"),
                            WHDeduction = Convert.ToInt32(worksheet.Cells[row, 19].Value?.ToString().Trim() ?? "0"),
                            WHITDeduction = Convert.ToInt32(worksheet.Cells[row, 20].Value?.ToString().Trim() ?? "0"),
                            TotalDeductions = Convert.ToInt32(worksheet.Cells[row, 21].Value?.ToString().Trim() ?? "0"),
                            NetAmount = Convert.ToInt32(worksheet.Cells[row, 22].Value?.ToString().Trim() ?? "0")
                        };
                        lines.Add(uploadFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing row {row}: {ex.Message}");
                    }
                }
            }

            return lines;
        }
        private async Task SendEmail(string recipientEmail, UploadFile file)
        {
            try
            {
                string subject = "Salary Details";
                string body = GenerateHtmlEmailBody(file);

                await emailService.SendEmailAsync(recipientEmail, subject, body);
                Console.WriteLine($"Email sent to {recipientEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {recipientEmail}: {ex.Message}");
            }
        }

        private string GenerateHtmlEmailBody(UploadFile file)
        {
            string htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    table {{
                        border-collapse: collapse;
                        width: 100%;
                    }}
                    th, td {{
                        border: 1px solid black;
                        padding: 8px;
                        text-align: left;
                    }}
                    th {{
                        background-color: #f2f2f2;
                    }}
                </style>
            </head>
            <body>
                <h2>Salary Details</h2>
                <table>
                    <tr><th>Employee Name:</th><td>{file.EmployeeName}</td></tr>
                    <tr><th>Employee ID:</th><td>{file.EmoployeeId}</td></tr>
                    <tr><th>Service Category:</th><td>{file.ServiceCategory}</td></tr>
                    <tr><th>CNIC:</th><td>{file.Cnic}</td></tr>
                    <tr><th>Email:</th><td>{file.Email}</td></tr>
                    <tr><th>Joining Date:</th><td>{file.JoiningDate}</td></tr>
                    <tr><th>Month Days:</th><td>{file.MonthDays}</td></tr>
                    <tr><th>Present Days:</th><td>{file.PresentDays}</td></tr>
                    <tr><th>Offered Salary:</th><td>{file.OfferedSalary}</td></tr>
                    <tr><th>Leave Deduction:</th><td>{file.LeaveDeduction}</td></tr>
                    <tr><th>Basic Pay After Deduction:</th><td>{file.BasicPayafterDeduction}</td></tr>
                    <tr><th>Arrears:</th><td>{file.Arrears}</td></tr>
                    <tr><th>Allowances:</th><td>{file.Allowances}</td></tr>
                    <tr><th>Advances Loan:</th><td>{file.AdvancesLoan}</td></tr>
                    <tr><th>Gross Salary:</th><td>{file.GrossSalary}</td></tr>
                    <tr><th>Advances Loan Deduction:</th><td>{file.AdvancesLoanDeduction}</td></tr>
                    <tr><th>EOBI:</th><td>{file.EOBI}</td></tr>
                    <tr><th>WH Deduction:</th><td>{file.WHDeduction}</td></tr>
                    <tr><th>WHIT Deduction:</th><td>{file.WHITDeduction}</td></tr>
                    <tr><th>Total Deductions:</th><td>{file.TotalDeductions}</td></tr>
                    <tr><th>Net Amount:</th><td>{file.NetAmount}</td></tr>
                </table>
            </body>
            </html>
        ";

            return htmlBody;
        }



    }

}