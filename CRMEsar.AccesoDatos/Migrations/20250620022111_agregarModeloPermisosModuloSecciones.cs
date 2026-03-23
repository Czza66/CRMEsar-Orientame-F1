using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class agregarModeloPermisosModuloSecciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PermisosModulosSecciones",
                columns: table => new
                {
                    PermisoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Temporal = table.Column<bool>(type: "bit", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeccionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModulosSeccionesseccionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModulosmoduloId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisosModulosSecciones", x => x.PermisoId);
                    table.ForeignKey(
                        name: "FK_PermisosModulosSecciones_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermisosModulosSecciones_AspNetRoles_RolId",
                        column: x => x.RolId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisosModulosSecciones_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisosModulosSecciones_ModulosSecciones_ModulosSeccionesseccionId",
                        column: x => x.ModulosSeccionesseccionId,
                        principalTable: "ModulosSecciones",
                        principalColumn: "seccionId");
                    table.ForeignKey(
                        name: "FK_PermisosModulosSecciones_ModulosSecciones_SeccionId",
                        column: x => x.SeccionId,
                        principalTable: "ModulosSecciones",
                        principalColumn: "seccionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisosModulosSecciones_Modulos_ModuloId",
                        column: x => x.ModuloId,
                        principalTable: "Modulos",
                        principalColumn: "moduloId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisosModulosSecciones_Modulos_ModulosmoduloId",
                        column: x => x.ModulosmoduloId,
                        principalTable: "Modulos",
                        principalColumn: "moduloId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_ApplicationRoleId",
                table: "PermisosModulosSecciones",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_ModuloId",
                table: "PermisosModulosSecciones",
                column: "ModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_ModulosmoduloId",
                table: "PermisosModulosSecciones",
                column: "ModulosmoduloId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_ModulosSeccionesseccionId",
                table: "PermisosModulosSecciones",
                column: "ModulosSeccionesseccionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_RolId",
                table: "PermisosModulosSecciones",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_SeccionId",
                table: "PermisosModulosSecciones",
                column: "SeccionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosModulosSecciones_UsuarioId",
                table: "PermisosModulosSecciones",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermisosModulosSecciones");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");
        }
    }
}
