using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class EliminarPermisosModulosSecciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermisosModulosSecciones_AspNetUsers_UsuarioId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropForeignKey(
                name: "FK_PermisosModulosSecciones_ModulosSecciones_ModulosSeccionesseccionId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropForeignKey(
                name: "FK_PermisosModulosSecciones_Modulos_ModulosmoduloId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropIndex(
                name: "IX_PermisosModulosSecciones_ModulosmoduloId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropIndex(
                name: "IX_PermisosModulosSecciones_ModulosSeccionesseccionId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropColumn(
                name: "ModulosSeccionesseccionId",
                table: "PermisosModulosSecciones");

            migrationBuilder.DropColumn(
                name: "ModulosmoduloId",
                table: "PermisosModulosSecciones");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "PermisosModulosSecciones",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PermisosModulosSecciones_UsuarioId",
                table: "PermisosModulosSecciones",
                newName: "IX_PermisosModulosSecciones_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermisosModulosSecciones_AspNetUsers_ApplicationUserId",
                table: "PermisosModulosSecciones",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermisosModulosSecciones_AspNetUsers_ApplicationUserId",
                table: "PermisosModulosSecciones");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "PermisosModulosSecciones",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_PermisosModulosSecciones_ApplicationUserId",
                table: "PermisosModulosSecciones",
                newName: "IX_PermisosModulosSecciones_UsuarioId");

            migrationBuilder.AddColumn<Guid>(
                name: "ModulosSeccionesseccionId",
                table: "PermisosModulosSecciones",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModulosmoduloId",
                table: "PermisosModulosSecciones",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_ModulosmoduloId",
                table: "PermisosModulosSecciones",
                column: "ModulosmoduloId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_ModulosSeccionesseccionId",
                table: "PermisosModulosSecciones",
                column: "ModulosSeccionesseccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermisosModulosSecciones_AspNetUsers_UsuarioId",
                table: "PermisosModulosSecciones",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermisosModulosSecciones_ModulosSecciones_ModulosSeccionesseccionId",
                table: "PermisosModulosSecciones",
                column: "ModulosSeccionesseccionId",
                principalTable: "ModulosSecciones",
                principalColumn: "seccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermisosModulosSecciones_Modulos_ModulosmoduloId",
                table: "PermisosModulosSecciones",
                column: "ModulosmoduloId",
                principalTable: "Modulos",
                principalColumn: "moduloId");
        }
    }
}
