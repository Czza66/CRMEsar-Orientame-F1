using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class correccionPaises2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paises_Estado_estadosEstadoId",
                table: "Paises");

            migrationBuilder.DropIndex(
                name: "IX_Paises_estadosEstadoId",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "estadosEstadoId",
                table: "Paises");

            migrationBuilder.CreateIndex(
                name: "IX_Paises_EstadoId",
                table: "Paises",
                column: "EstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paises_Estado_EstadoId",
                table: "Paises",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "EstadoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paises_Estado_EstadoId",
                table: "Paises");

            migrationBuilder.DropIndex(
                name: "IX_Paises_EstadoId",
                table: "Paises");

            migrationBuilder.AddColumn<Guid>(
                name: "estadosEstadoId",
                table: "Paises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Paises_estadosEstadoId",
                table: "Paises",
                column: "estadosEstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paises_Estado_estadosEstadoId",
                table: "Paises",
                column: "estadosEstadoId",
                principalTable: "Estado",
                principalColumn: "EstadoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
