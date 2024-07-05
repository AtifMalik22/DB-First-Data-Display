using Demo_Project.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Demo_Project.Pages
{
    public partial class Uploader
    {
        private List<UploadFile> fileContent=new();
        protected override async Task OnInitializedAsync()
        {
            fileContent = await uploadFileService.GetDataAsync();
        }
        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;

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
                await uploadFileService.SaveFileContentToDatabase(fileContent);
                //StateHasChanged();
            }
        }
        //private async Task HandleFileSelected(InputFileChangeEventArgs e)
        //{
        //    var file = e.File;

        //    // Check if the file is an Excel file
        //    if (!file.Name.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        //    {
        //        // Handle incorrect file type
        //        Console.WriteLine("Uploaded file is not an Excel file (.xlsx).");
        //        return;
        //    }

        //    var filePath = Path.Combine(Environment.WebRootPath, "UploadFiles", file.Name);

        //    Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Ensure directory exists

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.OpenReadStream().CopyToAsync(stream);
        //    }

        //    fileContent = ReadExcelFile(filePath);

        //    if (fileContent != null && fileContent.Any())
        //    {
        //        await uploadFileService.SaveFileContentToDatabase(fileContent);
        //        // Refresh UI or perform necessary actions to display data in Razor page table
        //        StateHasChanged(); // Ensures UI updates are triggered
        //    }
        //}


        private List<UploadFile> ReadExcelFile(string filePath)
        {
            var lines = new List<UploadFile>();

            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
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




    }

}