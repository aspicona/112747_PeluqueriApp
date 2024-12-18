using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeluqueriApp.Migrations
{
    public partial class AddCostoInsumosToServicios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostoInsumos",
                table: "Servicios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostoInsumos",
                table: "Servicios");
        }
    }
}
