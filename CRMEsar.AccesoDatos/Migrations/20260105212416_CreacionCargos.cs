using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CreacionCargos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    CargoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAntiguo = table.Column<int>(type: "int", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.CargoId);
                    table.ForeignKey(
                        name: "FK_Cargos_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_EstadoId",
                table: "Cargos",
                column: "EstadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cargos");
        }
    }
}
