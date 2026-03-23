using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class SeAgregaModeloIntegraciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Integraciones",
                columns: table => new
                {
                    IntegracionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoIntegracion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndpointURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    metodoHttp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JsonSchema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integraciones", x => x.IntegracionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Integraciones");
        }
    }
}
