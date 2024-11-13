using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeluqueriApp.Migrations
{
    public partial class AddMetodoDePago : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Pagos");

            migrationBuilder.AddColumn<int>(
                name: "MetodoDePagoId",
                table: "Pagos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MetodosDePago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodosDePago", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_MetodoDePagoId",
                table: "Pagos",
                column: "MetodoDePagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_MetodosDePago_MetodoDePagoId",
                table: "Pagos",
                column: "MetodoDePagoId",
                principalTable: "MetodosDePago",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_MetodosDePago_MetodoDePagoId",
                table: "Pagos");

            migrationBuilder.DropTable(
                name: "MetodosDePago");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_MetodoDePagoId",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "MetodoDePagoId",
                table: "Pagos");

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Pagos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
