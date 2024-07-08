using Demo_Project.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OfficeOpenXml;
using Syncfusion.Blazor.Diagrams;
using Syncfusion.Blazor.Inputs;

namespace Demo_Project.Pages
{
    public partial class Uploader
    {
        UploadFile filess = new();
        private List<UploadFile> fileContent = new();
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
                        .table th, .table {{ border: 1px solid black; padding: 8px; text-align: left; }}
                        .table td{{padding:8px;}}
                        .table th, .table .td1 {{ border: 1px solid black; padding: 8px; text-align: left; }}
                        .table td{{border-right: 1px solid blackl;}}
                        //.table th {{ background-color: #f2f2f2; }}
                        .header, .footer {{ text-align: center; margin-top: 20px; }}
                        .header p, .footer p {{ margin: 0; }}
                        .header {{display: flex;justify-content: space-between;
                         align-items: center; /* Optional: Centers items vertically */
                         padding: 10px;/*background-color: #f0f0f0;*/ /* Optional: Background color */}}
                        .header {{display: flex;justify-content: space-between;
                         align-items: center;padding: 10px;/*background-color: #f0f0f0;*/}}
                        .header > div {{display: flex;justify-content: space-between;}}
                        .float-left{{text-align:left;}}
                    </style>
                </head>
                <body style='border-left:5px solid orange;'>
                    <div class='header' style='display: flex; justify-content: space-between; padding: 10px;'>
                        <div>
                            <p style='text-align:left'>Logo</p>
                        </div>
                        <div style='text-align: right;'>
                            <p>Solochoicez (Pvt) Ltd.</p>
                            <p>Office # 9,2nd Floor,<br/>  VIP Plaza, I-8 Markaz,<br/>  Islamabad, Pakistan.</p>
                            <p>Tel: +92 51 8446629</p>
                            <p>Email: info@solochoicez.com</p>
                            <p>Web: www.solochoicez.com</p>
                        </div>
                    </div>

                    <h2 style='text-align: center;color:#054B71;'>Salary Slip</h2>
                    <p style='text-align: center; color:#054B71;'>For the Month - {file.Date.ToString("MMMM yyyy")}</p>
        <hr/>
        <div style='display:flex; justify-content:between;'>
        <div class='d1'><p style='text-align:left'>Employee ID: {file.EmoployeeId}</p></div>
        <div class='d2'><p style='text-align:righ;'>Employee Name: {file.EmployeeName}</p></div>
        </div>

        <hr/>
                    <p>Designation :</p>
                    <p>CNIC: {file.Cnic}</p>
                    <table class='table'>
                        <tr style='background-color:#054B71;color:white;'><th>ADDITIONS</th><th>Amount (Rs.)</th><th>DEDUCTIONS</th><th>Amount (Rs.)</th></tr>
                        <tr><td>Gross Pay</td><td>{file.GrossSalary}</td><td>Advance Payments</td><td>{file.AdvancesLoanDeduction}</td></tr>
                        <tr><td>Arrears</td><td>{file.Arrears}</td><td>Leave Deduction</td><td>{file.LeaveDeduction}</td></tr>
                        <tr><td>Travelling Allowance</td><td>-</td><td>Income Tax</td><td>{file.WHITDeduction}</td></tr>
                        <tr><td>Medical Allowance</td><td>-</td><td>EOBI</td><td>{file.EOBI}</td></tr>
                        <tr><td>Equipment Allowance</td><td>-</td><td></td><td>-</td></tr>
                        <tr><td>Communication Allowance</td><td>-</td><td></td><td>-</td></tr>
                        <tr><td class='td1'>Total Additions</td><td class='td1'></td><td class='td1'>Total Deductions</td><td class='td1'>{file.TotalDeductions}</td></tr>
                        <tr><td colspan='3' style='text-align:right;'>Net Pay</td><td>{file.NetAmount}</td></tr>
                    </table>
                    <div class='footer float-left'>
                    <img style='width:20px; height:20px;' src='/wwwroot/time-and-stamp.png' alt='Logo' />
                        <p><strong>Shehzeela Shafique</strong></p>
                        <p>HR Executive / POC at NITB</p>
                    </div>
                </body>
                </html>";
        }
    }

}