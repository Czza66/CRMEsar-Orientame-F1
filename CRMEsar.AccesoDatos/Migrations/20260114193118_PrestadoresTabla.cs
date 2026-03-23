using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class PrestadoresTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prestadores",
                columns: table => new
                {
                    PrestadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAntiguo = table.Column<int>(type: "int", nullable: false),
                    Profesion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    RecomendadoPor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Celular = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Orientador = table.Column<bool>(type: "bit", nullable: false),
                    PermiteILVE = table.Column<bool>(type: "bit", nullable: false),
                    TipoServicio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TipoPrestador = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestadores", x => x.PrestadorId);
                    table.ForeignKey(
                        name: "FK_Prestadores_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestadores_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prestadores_EstadoId",
                table: "Prestadores",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestadores_UserID",
                table: "Prestadores",
                column: "UserID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestadores");
        }
    }
}
