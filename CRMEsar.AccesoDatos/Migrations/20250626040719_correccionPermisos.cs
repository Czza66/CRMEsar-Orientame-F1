using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class correccionPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermisosModulosSecciones_AspNetUsers_ApplicationUserId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropIndex(
                name: "IX_PermisosModulosSecciones_ApplicationUserId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "PermisosModulosSecciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "PermisosModulosSecciones",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_ApplicationUserId",
                table: "PermisosModulosSecciones",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermisosModulosSecciones_AspNetUsers_ApplicationUserId",
                table: "PermisosModulosSecciones",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
