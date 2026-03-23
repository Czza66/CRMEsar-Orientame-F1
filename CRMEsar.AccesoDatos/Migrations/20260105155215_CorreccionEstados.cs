using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionEstados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_Entidad_EntidadesEntidadId",
                table: "Estado");

            migrationBuilder.DropIndex(
                name: "IX_Estado_EntidadesEntidadId",
                table: "Estado");

            migrationBuilder.DropColumn(
                name: "EntidadesEntidadId",
                table: "Estado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EntidadesEntidadId",
                table: "Estado",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estado_EntidadesEntidadId",
                table: "Estado",
                column: "EntidadesEntidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_Entidad_EntidadesEntidadId",
                table: "Estado",
                column: "EntidadesEntidadId",
                principalTable: "Entidad",
                principalColumn: "EntidadId");
        }
    }
}
