using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CreacionEntidadModulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modulos",
                columns: table => new
                {
                    moduloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    icono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcionCorta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    orden = table.Column<int>(type: "int", nullable: false),
                    visible = table.Column<bool>(type: "bit", nullable: false),
                    EstadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulos", x => x.moduloId);
                    table.ForeignKey(
                        name: "FK_Modulos_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "EstadoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modulos_EstadoId",
                table: "Modulos",
                column: "EstadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modulos");
        }
    }
}
