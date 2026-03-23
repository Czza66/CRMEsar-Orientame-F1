using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class modificacionModuloSecciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_Entidad_EntidadId",
                table: "Estado");

            migrationBuilder.DropForeignKey(
                name: "FK_Modulos_Estado_EstadoId",
                table: "Modulos");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulosSecciones_Estado_EstadoId",
                table: "ModulosSecciones");

            migrationBuilder.AddColumn<Guid>(
                name: "ModuloId",
                table: "ModulosSecciones",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "seccionPadreId",
                table: "ModulosSecciones",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntidadesEntidadId",
                table: "Estado",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulosSecciones_ModuloId",
                table: "ModulosSecciones",
                column: "ModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulosSecciones_seccionPadreId",
                table: "ModulosSecciones",
                column: "seccionPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Estado_EntidadesEntidadId",
                table: "Estado",
                column: "EntidadesEntidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_Entidad_EntidadId",
                table: "Estado",
                column: "EntidadId",
                principalTable: "Entidad",
                principalColumn: "EntidadId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_Entidad_EntidadesEntidadId",
                table: "Estado",
                column: "EntidadesEntidadId",
                principalTable: "Entidad",
                principalColumn: "EntidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modulos_Estado_EstadoId",
                table: "Modulos",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "EstadoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulosSecciones_Estado_EstadoId",
                table: "ModulosSecciones",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "EstadoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulosSecciones_ModulosSecciones_seccionPadreId",
                table: "ModulosSecciones",
                column: "seccionPadreId",
                principalTable: "ModulosSecciones",
                principalColumn: "seccionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulosSecciones_Modulos_ModuloId",
                table: "ModulosSecciones",
                column: "ModuloId",
                principalTable: "Modulos",
                principalColumn: "moduloId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_Entidad_EntidadId",
                table: "Estado");

            migrationBuilder.DropForeignKey(
                name: "FK_Estado_Entidad_EntidadesEntidadId",
                table: "Estado");

            migrationBuilder.DropForeignKey(
                name: "FK_Modulos_Estado_EstadoId",
                table: "Modulos");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulosSecciones_Estado_EstadoId",
                table: "ModulosSecciones");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulosSecciones_ModulosSecciones_seccionPadreId",
                table: "ModulosSecciones");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulosSecciones_Modulos_ModuloId",
                table: "ModulosSecciones");

            migrationBuilder.DropIndex(
                name: "IX_ModulosSecciones_ModuloId",
                table: "ModulosSecciones");

            migrationBuilder.DropIndex(
                name: "IX_ModulosSecciones_seccionPadreId",
                table: "ModulosSecciones");

            migrationBuilder.DropIndex(
                name: "IX_Estado_EntidadesEntidadId",
                table: "Estado");

            migrationBuilder.DropColumn(
                name: "ModuloId",
                table: "ModulosSecciones");

            migrationBuilder.DropColumn(
                name: "seccionPadreId",
                table: "ModulosSecciones");

            migrationBuilder.DropColumn(
                name: "EntidadesEntidadId",
                table: "Estado");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_Entidad_EntidadId",
                table: "Estado",
                column: "EntidadId",
                principalTable: "Entidad",
                principalColumn: "EntidadId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modulos_Estado_EstadoId",
                table: "Modulos",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "EstadoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulosSecciones_Estado_EstadoId",
                table: "ModulosSecciones",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "EstadoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
