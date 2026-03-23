using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionUsuarios2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CargoId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TiposDocumentosId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CargoId",
                table: "AspNetUsers",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TiposDocumentosId",
                table: "AspNetUsers",
                column: "TiposDocumentosId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cargos_CargoId",
                table: "AspNetUsers",
                column: "CargoId",
                principalTable: "Cargos",
                principalColumn: "CargoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TipoDocumentos_TiposDocumentosId",
                table: "AspNetUsers",
                column: "TiposDocumentosId",
                principalTable: "TipoDocumentos",
                principalColumn: "TiposDocumentosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cargos_CargoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TipoDocumentos_TiposDocumentosId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CargoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TiposDocumentosId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CargoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TiposDocumentosId",
                table: "AspNetUsers");
        }
    }
}
