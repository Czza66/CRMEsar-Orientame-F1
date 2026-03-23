using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEsar.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AddMigrationCorreccionEstadosyUsarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CuentaID",
                table: "AspNetUsers",
                newName: "IdAntiguo");

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoUsuario",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TipoUsuario",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IdAntiguo",
                table: "AspNetUsers",
                newName: "CuentaID");
        }
    }
}
