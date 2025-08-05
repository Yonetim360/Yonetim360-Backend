using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yonetim360.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mg_19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_CustomerSupportRequests_CustomerSupportRequestId",
                table: "Representatives");

            migrationBuilder.DropIndex(
                name: "IX_Representatives_CustomerSupportRequestId",
                table: "Representatives");

            migrationBuilder.DropColumn(
                name: "CustomerSupportRequestId",
                table: "Representatives");

            migrationBuilder.CreateTable(
                name: "CustomerSupportRequestRepresentative",
                columns: table => new
                {
                    CustomerSupportRequestsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepresentativesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSupportRequestRepresentative", x => new { x.CustomerSupportRequestsId, x.RepresentativesId });
                    table.ForeignKey(
                        name: "FK_CustomerSupportRequestRepresentative_CustomerSupportRequests_CustomerSupportRequestsId",
                        column: x => x.CustomerSupportRequestsId,
                        principalTable: "CustomerSupportRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSupportRequestRepresentative_Representatives_RepresentativesId",
                        column: x => x.RepresentativesId,
                        principalTable: "Representatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupportRequestRepresentative_RepresentativesId",
                table: "CustomerSupportRequestRepresentative",
                column: "RepresentativesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSupportRequestRepresentative");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerSupportRequestId",
                table: "Representatives",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Representatives_CustomerSupportRequestId",
                table: "Representatives",
                column: "CustomerSupportRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_CustomerSupportRequests_CustomerSupportRequestId",
                table: "Representatives",
                column: "CustomerSupportRequestId",
                principalTable: "CustomerSupportRequests",
                principalColumn: "Id");
        }
    }
}
