using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yonetim360.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mg_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrmDepartment",
                table: "Representatives");

            migrationBuilder.AddColumn<int>(
                name: "TaxIncluded",
                table: "Offers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MeetingType",
                table: "Conversation",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxIncluded",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "MeetingType",
                table: "Conversation");

            migrationBuilder.AddColumn<int>(
                name: "CrmDepartment",
                table: "Representatives",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
