using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class correccionTipoNotificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_TipoNotificaciones_TipoNotificacionestipoNotificacionId",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_TipoNotificacionestipoNotificacionId",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "TipoNotificacionestipoNotificacionId",
                table: "Notificaciones");

            migrationBuilder.RenameColumn(
                name: "notificacionID",
                table: "Notificaciones",
                newName: "NotificacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_TipoNotificacionId",
                table: "Notificaciones",
                column: "TipoNotificacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_TipoNotificaciones_TipoNotificacionId",
                table: "Notificaciones",
                column: "TipoNotificacionId",
                principalTable: "TipoNotificaciones",
                principalColumn: "tipoNotificacionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_TipoNotificaciones_TipoNotificacionId",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_TipoNotificacionId",
                table: "Notificaciones");

            migrationBuilder.RenameColumn(
                name: "NotificacionId",
                table: "Notificaciones",
                newName: "notificacionID");

            migrationBuilder.AddColumn<Guid>(
                name: "TipoNotificacionestipoNotificacionId",
                table: "Notificaciones",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_TipoNotificacionestipoNotificacionId",
                table: "Notificaciones",
                column: "TipoNotificacionestipoNotificacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_TipoNotificaciones_TipoNotificacionestipoNotificacionId",
                table: "Notificaciones",
                column: "TipoNotificacionestipoNotificacionId",
                principalTable: "TipoNotificaciones",
                principalColumn: "tipoNotificacionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
