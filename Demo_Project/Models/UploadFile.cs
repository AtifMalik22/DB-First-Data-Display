using Microsoft.EntityFrameworkCore;

namespace Demo_Project.Models
{
    public class UploadFile
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string? EmoployeeId { get; set; }
        public string? ServiceCategory { get; set; }
        public string? EmployeeName { get; set; }
        public string? Cnic { get; set; }
        public string? Email { get; set; }
        public string? JoiningDate { get; set; }
        public int? MonthDays { get; set; }
        public int? PresentDays { get; set; }
        public int? OfferedSalary { get; set; }
        public int? LeaveDeduction { get; set; }
        public int? BasicPayafterDeduction { get; set; }
        public int? Arrears { get; set; }
        public int? Allowances { get; set; }
        public int? AdvancesLoan { get; set; }
        public int? GrossSalary { get; set; }
        public int? AdvancesLoanDeduction { get; set; }
        public int? EOBI { get; set; }
        public int? WHDeduction { get; set; }
        public int? WHITDeduction { get; set; }
        public int? TotalDeductions { get; set; }
        public int? NetAmount { get; set; }

    }
}
