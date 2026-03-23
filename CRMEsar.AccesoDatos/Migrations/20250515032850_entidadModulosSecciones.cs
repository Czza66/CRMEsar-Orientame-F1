using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class entidadModulosSecciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modulos_Estado_EstadoId",
                table: "Modulos");

            migrationBuilder.AlterColumn<Guid>(
                name: "EstadoId",
                table: "Modulos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ModulosSecciones",
                columns: table => new
                {
                    seccionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    orden = table.Column<int>(type: "int", nullable: false),
                    visible = table.Column<bool>(type: "bit", nullable: false),
                    area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    controller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulosSecciones", x => x.seccionId);
                    table.ForeignKey(
                        name: "FK_ModulosSecciones_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModulosSecciones_EstadoId",
                table: "ModulosSecciones",
                column: "EstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modulos_Estado_EstadoId",
                table: "Modulos",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "EstadoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modulos_Estado_EstadoId",
                table: "Modulos");

            migrationBuilder.DropTable(
                name: "ModulosSecciones");

            migrationBuilder.AlterColumn<Guid>(
                name: "EstadoId",
                table: "Modulos",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Modulos_Estado_EstadoId",
                table: "Modulos",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "EstadoId");
        }
    }
}
