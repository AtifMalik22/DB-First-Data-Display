using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoProject.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SrNo = table.Column<int>(type: "int", nullable: false),
                    EmoployeeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cnic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoiningDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthDays = table.Column<int>(type: "int", nullable: true),
                    PresentDays = table.Column<int>(type: "int", nullable: true),
                    OfferedSalary = table.Column<int>(type: "int", nullable: true),
                    LeaveDeduction = table.Column<int>(type: "int", nullable: true),
                    BasicPayafterDeduction = table.Column<int>(type: "int", nullable: true),
                    Arrears = table.Column<int>(type: "int", nullable: true),
                    Allowances = table.Column<int>(type: "int", nullable: true),
                    AdvancesLoan = table.Column<int>(type: "int", nullable: true),
                    GrossSalary = table.Column<int>(type: "int", nullable: true),
                    AdvancesLoanDeduction = table.Column<int>(type: "int", nullable: true),
                    EOBI = table.Column<int>(type: "int", nullable: true),
                    WHDeduction = table.Column<int>(type: "int", nullable: true),
                    WHITDeduction = table.Column<int>(type: "int", nullable: true),
                    TotalDeductions = table.Column<int>(type: "int", nullable: true),
                    NetAmount = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "v_Accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountnum = table.Column<string>(name: "account_num", type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parentid = table.Column<int>(name: "parent_id", type: "int", nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    endingbalanceamt = table.Column<decimal>(name: "ending_balance_amt", type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v_Accounts", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "v_Accounts");
        }
    }
}
