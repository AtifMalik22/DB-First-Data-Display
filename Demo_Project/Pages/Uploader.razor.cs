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
        private async Task SendEmailWithPdf(UploadFile file)
        {
            string subject = "Payroll Details";
            string body = "Please find the attached payroll details in PDF format.";

            string pdfContent = GeneratePdfContent(file);

            await emailService.SendEmailWithPdfAsync(file.Email, subject, body, pdfContent);
        }

        private string GeneratePdfContent(UploadFile file)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        .table {{ width: 100%; border-collapse: collapse; }}
        .table th, .table td {{ border: 1px solid black; padding: 8px; text-align: left; }}
        .table th {{ background-color: #f2f2f2; }}
        .header, .footer {{ text-align: center; margin-top: 20px; }}
        .header p, .footer p {{ margin: 0; }}
    </style>
</head>
<body>
    <div class='header'>
        <p>Solochoicez (Pvt) Ltd.</p>
        <p>Office # 9, 2nd Floor, VIP Plaza, I-8 Markaz, Islamabad, Pakistan.</p>
        <p>Tel:+ 92 51 8446629</p>
        <p>Email: info@solochoicez.com</p>
        <p>Web: www.solochoicez.com</p>
    </div>
    <h2 style='text-align: center;'>Salary Slip</h2>
    <p style='text-align: center;'>For the Month - June 2024</p>
    <p>Employee ID: {file.EmoployeeId}</p>
    <p>Employee Name: {file.EmployeeName}</p>
    <p>Designation : </p>
    <p>CNIC: {file.Cnic}</p>
    <table class='table'>
        <tr><th>Amount (Rs.)</th><th>DEDUCTIONS</th><th>Amount (Rs.)</th></tr>
        <tr><td>{file.OfferedSalary}</td><td>Advance Payment</td><td>-</td></tr>
        <tr><td>-</td><td>Leave Deduction</td><td>{file.LeaveDeduction}</td></tr>
        <tr><td>-</td><td>Income Tax</td><td>{file.WHITDeduction}</td></tr>
        <tr><td>-</td><td>EOBI</td><td>{file.EOBI}</td></tr>
        <tr><td>-</td><td>-</td><td>-</td></tr>
        <tr><td>{file.GrossSalary}</td><td>Total Deductions</td><td>{file.TotalDeductions}</td></tr>
        <tr><td>{file.NetAmount}</td><td></td><td></td></tr>
    </table>
    <div class='footer'>
        <p>Shehzeela Shafique</p>
        <p>HR Executive / POC at NITB</p>
    </div>
</body>
</html>";
        }





    }

}