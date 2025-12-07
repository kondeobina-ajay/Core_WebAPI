using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeToAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeId",
                table: "Attendance",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Employees_EmployeeId",
                table: "Attendance",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Employees_EmployeeId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_EmployeeId",
                table: "Attendance");
        }
    }
}
