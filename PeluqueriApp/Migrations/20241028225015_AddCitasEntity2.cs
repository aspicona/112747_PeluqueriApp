using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeluqueriApp.Migrations
{
    public partial class AddCitasEntity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosCita",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCita", x => x.Id);
                });

            migrationBuilder.CreateTable(
    name: "Citas",
    columns: table => new
    {
        Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        IdCliente = table.Column<int>(type: "int", nullable: false),
        Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
        IdEstadoCita = table.Column<int>(type: "int", nullable: false),
        PrecioEstimado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
        PrecioFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
        IdEmpleado = table.Column<int>(type: "int", nullable: false),
        IdEmpresa = table.Column<int>(type: "int", nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Citas", x => x.Id);
        table.ForeignKey(
            name: "FK_Citas_Clientes_IdCliente",
            column: x => x.IdCliente,
            principalTable: "Clientes",
            principalColumn: "Id",
            onDelete: ReferentialAction.NoAction);
        table.ForeignKey(
            name: "FK_Citas_Empleados_IdEmpleado",
            column: x => x.IdEmpleado,
            principalTable: "Empleados",
            principalColumn: "Id",
            onDelete: ReferentialAction.NoAction);
        table.ForeignKey(
            name: "FK_Citas_Empresas_IdEmpresa",
            column: x => x.IdEmpresa,
            principalTable: "Empresas",
            principalColumn: "Id",
            onDelete: ReferentialAction.NoAction); // Cambia a NO ACTION
        table.ForeignKey(
            name: "FK_Citas_EstadosCita_IdEstadoCita",
            column: x => x.IdEstadoCita,
            principalTable: "EstadosCita",
            principalColumn: "Id",
            onDelete: ReferentialAction.NoAction);
    });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IdCliente",
                table: "Citas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IdEmpleado",
                table: "Citas",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IdEmpresa",
                table: "Citas",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IdEstadoCita",
                table: "Citas",
                column: "IdEstadoCita");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "EstadosCita");
        }
    }
}
