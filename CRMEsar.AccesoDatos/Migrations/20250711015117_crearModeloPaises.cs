using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class crearModeloPaises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaisId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    PaisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codigoInternacional = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.PaisId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PaisId",
                table: "AspNetUsers",
                column: "PaisId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Paises_PaisId",
                table: "AspNetUsers",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "PaisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Paises_PaisId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PaisId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "AspNetUsers");
        }
    }
}
