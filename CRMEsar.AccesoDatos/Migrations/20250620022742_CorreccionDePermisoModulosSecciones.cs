using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionDePermisoModulosSecciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermisosModulosSecciones_AspNetRoles_ApplicationRoleId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropIndex(
                name: "IX_PermisosModulosSecciones_ApplicationRoleId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                table: "PermisosModulosSecciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationRoleId",
                table: "PermisosModulosSecciones",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_ApplicationRoleId",
                table: "PermisosModulosSecciones",
                column: "ApplicationRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermisosModulosSecciones_AspNetRoles_ApplicationRoleId",
                table: "PermisosModulosSecciones",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}
