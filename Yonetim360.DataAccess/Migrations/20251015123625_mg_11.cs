using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yonetim360.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mg_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Representatives_RepresentativeId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_RepresentativeId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Tasks",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CrmTaskRepresentative",
                columns: table => new
                {
                    CrmTasksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepresentativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmTaskRepresentative", x => new { x.CrmTasksId, x.RepresentativeId });
                    table.ForeignKey(
                        name: "FK_CrmTaskRepresentative_Representatives_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "Representatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrmTaskRepresentative_Tasks_CrmTasksId",
                        column: x => x.CrmTasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrmTaskRepresentative_RepresentativeId",
                table: "CrmTaskRepresentative",
                column: "RepresentativeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrmTaskRepresentative");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Tasks",
                newName: "Time");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_RepresentativeId",
                table: "Tasks",
                column: "RepresentativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Representatives_RepresentativeId",
                table: "Tasks",
                column: "RepresentativeId",
                principalTable: "Representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
