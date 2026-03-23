using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionModeloLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogs_AspNetActionLogs_AccionActionLogId",
                table: "AspNetUserLogs");

            migrationBuilder.DropTable(
                name: "AspNetActionLogs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogs_AccionActionLogId",
                table: "AspNetUserLogs");

            migrationBuilder.DropColumn(
                name: "AccionActionLogId",
                table: "AspNetUserLogs");

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "AspNetUserLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccionActionLogId",
                table: "AspNetUserLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ActionId",
                table: "AspNetUserLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AspNetActionLogs",
                columns: table => new
                {
                    ActionLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetActionLogs", x => x.ActionLogId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogs_AccionActionLogId",
                table: "AspNetUserLogs",
                column: "AccionActionLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogs_AspNetActionLogs_AccionActionLogId",
                table: "AspNetUserLogs",
                column: "AccionActionLogId",
                principalTable: "AspNetActionLogs",
                principalColumn: "ActionLogId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
