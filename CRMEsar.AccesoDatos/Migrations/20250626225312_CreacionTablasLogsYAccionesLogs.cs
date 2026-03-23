using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CreacionTablasLogsYAccionesLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "AspNetUserLogs",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreTabla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exitoso = table.Column<bool>(type: "bit", nullable: false),
                    Respuesta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccionActionLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_AspNetUserLogs_AspNetActionLogs_AccionActionLogId",
                        column: x => x.AccionActionLogId,
                        principalTable: "AspNetActionLogs",
                        principalColumn: "ActionLogId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogs_AccionActionLogId",
                table: "AspNetUserLogs",
                column: "AccionActionLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogs_UserId",
                table: "AspNetUserLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserLogs");

            migrationBuilder.DropTable(
                name: "AspNetActionLogs");
        }
    }
}
