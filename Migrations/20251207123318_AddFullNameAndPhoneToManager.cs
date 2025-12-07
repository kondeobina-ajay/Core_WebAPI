using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFullNameAndPhoneToManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Managers");
        }
    }
}
