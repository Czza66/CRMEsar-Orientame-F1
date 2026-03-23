using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class TiposDocumentos_OK_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoDocumentos",
                columns: table => new
                {
                    TiposDocumentosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAntiguo = table.Column<int>(type: "int", nullable: false),
                    TipoDocumento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PaisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumentos", x => x.TiposDocumentosId);
                    table.ForeignKey(
                        name: "FK_TipoDocumentos_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TipoDocumentos_Paises_PaisId",
                        column: x => x.PaisId,
                        principalTable: "Paises",
                        principalColumn: "PaisId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumentos_EstadoId",
                table: "TipoDocumentos",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumentos_PaisId",
                table: "TipoDocumentos",
                column: "PaisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoDocumentos");
        }
    }
}
