using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class SeCreanLosModelosNotificacionesYTiposNotifi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoNotificaciones",
                columns: table => new
                {
                    tipoNotificacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorHexadecimal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    icono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoNotificaciones", x => x.tipoNotificacionId);
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    notificacionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreTabla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstaLeido = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoNotificacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoNotificacionestipoNotificacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.notificacionID);
                    table.ForeignKey(
                        name: "FK_Notificaciones_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notificaciones_TipoNotificaciones_TipoNotificacionestipoNotificacionId",
                        column: x => x.TipoNotificacionestipoNotificacionId,
                        principalTable: "TipoNotificaciones",
                        principalColumn: "tipoNotificacionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_TipoNotificacionestipoNotificacionId",
                table: "Notificaciones",
                column: "TipoNotificacionestipoNotificacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UserId",
                table: "Notificaciones",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "TipoNotificaciones");
        }
    }
}
