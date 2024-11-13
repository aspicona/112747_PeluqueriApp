using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeluqueriApp.Migrations
{
    public partial class AddUnidadDeMedidaModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Insumos");

            migrationBuilder.AddColumn<int>(
                name: "UnidadDeMedidaId",
                table: "Insumos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UnidadesDeMedida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesDeMedida", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_UnidadDeMedidaId",
                table: "Insumos",
                column: "UnidadDeMedidaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insumos_UnidadesDeMedida_UnidadDeMedidaId",
                table: "Insumos",
                column: "UnidadDeMedidaId",
                principalTable: "UnidadesDeMedida",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insumos_UnidadesDeMedida_UnidadDeMedidaId",
                table: "Insumos");

            migrationBuilder.DropTable(
                name: "UnidadesDeMedida");

            migrationBuilder.DropIndex(
                name: "IX_Insumos_UnidadDeMedidaId",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "UnidadDeMedidaId",
                table: "Insumos");

            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "Insumos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
