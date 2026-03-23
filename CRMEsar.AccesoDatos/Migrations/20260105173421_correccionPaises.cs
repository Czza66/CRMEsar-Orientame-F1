using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class correccionPaises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Paises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "EstadoId",
                table: "Paises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "IdAntiguo",
                table: "Paises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteILVE",
                table: "Paises",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paises_Estado_estadosEstadoId",
                table: "Paises");

            migrationBuilder.DropIndex(
                name: "IX_Paises_estadosEstadoId",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "EstadoId",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "IdAntiguo",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "PermiteILVE",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "estadosEstadoId",
                table: "Paises");
        }
    }
}
