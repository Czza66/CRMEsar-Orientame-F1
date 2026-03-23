using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AddAtencionesRDA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AtencionesRDA",
                columns: table => new
                {
                    AtencionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAntiguo = table.Column<int>(type: "int", nullable: false),
                    CodigoPrestador = table.Column<int>(type: "int", nullable: false),
                    tipoPrestador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombrePrestador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    profesion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Donante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proyecto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCarga = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaAtencion = table.Column<DateOnly>(type: "date", nullable: false),
                    IdentidadDisociada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    GrupoEtareo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoConsulta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetodoAnticonceptivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAnticonceptivo = table.Column<DateOnly>(type: "date", nullable: true),
                    MacLargaDuracion = table.Column<bool>(type: "bit", nullable: false),
                    MacModerno = table.Column<bool>(type: "bit", nullable: false),
                    MedioRemision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetodoAnticonceptivoMac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAnticonceptivoMac = table.Column<DateOnly>(type: "date", nullable: true),
                    Utero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weeks = table.Column<int>(type: "int", nullable: false),
                    ILVE = table.Column<bool>(type: "bit", nullable: true),
                    Telemedicina = table.Column<bool>(type: "bit", nullable: true),
                    PrestadorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtencionesRDA", x => x.AtencionID);
                    table.ForeignKey(
                        name: "FK_AtencionesRDA_Prestadores_PrestadorID",
                        column: x => x.PrestadorID,
                        principalTable: "Prestadores",
                        principalColumn: "PrestadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtencionesRDA_PrestadorID",
                table: "AtencionesRDA",
                column: "PrestadorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtencionesRDA");
        }
    }
}
