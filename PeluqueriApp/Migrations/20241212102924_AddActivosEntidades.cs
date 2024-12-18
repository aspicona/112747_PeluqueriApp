using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeluqueriApp.Migrations
{
    public partial class AddActivosEntidades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Servicios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Insumos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Empresas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Empleados",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Citas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Servicios");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Citas");
        }
    }
}
