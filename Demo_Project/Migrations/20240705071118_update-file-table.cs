using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoProject.Migrations
{
    /// <inheritdoc />
    public partial class updatefiletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Files",
                newName: "WHITDeduction");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Files",
                newName: "ServiceCategory");

            migrationBuilder.AddColumn<int>(
                name: "AdvancesLoan",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdvancesLoanDeduction",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Allowances",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Arrears",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BasicPayafterDeduction",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Cnic",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EOBI",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmoployeeId",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GrossSalary",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JoiningDate",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LeaveDeduction",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MonthDays",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NetAmount",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OfferedSalary",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PresentDays",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SrNo",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalDeductions",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WHDeduction",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvancesLoan",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AdvancesLoanDeduction",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Allowances",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Arrears",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "BasicPayafterDeduction",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Cnic",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "EOBI",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "EmoployeeId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "GrossSalary",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "LeaveDeduction",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "MonthDays",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "OfferedSalary",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "PresentDays",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "SrNo",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "TotalDeductions",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "WHDeduction",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "WHITDeduction",
                table: "Files",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "ServiceCategory",
                table: "Files",
                newName: "Content");
        }
    }
}
