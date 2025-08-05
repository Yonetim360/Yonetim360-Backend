using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yonetim360.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mg_18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerSupportRequestId",
                table: "Representatives",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerSupportRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSupportRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSupportRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Representatives_CustomerSupportRequestId",
                table: "Representatives",
                column: "CustomerSupportRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupportRequests_CustomerId",
                table: "CustomerSupportRequests",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_CustomerSupportRequests_CustomerSupportRequestId",
                table: "Representatives",
                column: "CustomerSupportRequestId",
                principalTable: "CustomerSupportRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_CustomerSupportRequests_CustomerSupportRequestId",
                table: "Representatives");

            migrationBuilder.DropTable(
                name: "CustomerSupportRequests");

            migrationBuilder.DropIndex(
                name: "IX_Representatives_CustomerSupportRequestId",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "CustomerSupportRequestId",
                table: "Representatives");
        }
    }
}
