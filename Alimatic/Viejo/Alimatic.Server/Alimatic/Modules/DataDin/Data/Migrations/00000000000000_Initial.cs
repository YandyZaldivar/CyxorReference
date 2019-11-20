using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Alimatic.DataDin.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Divisiones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisiones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modelos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CantidadColumnas = table.Column<int>(nullable: false),
                    CantidadFilas = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    DivisionId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => new { x.DivisionId, x.Id });
                    table.ForeignKey(
                        name: "FK_Grupos_Divisiones_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Filas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ModeloId = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filas", x => new { x.Id, x.ModeloId });
                    table.ForeignKey(
                        name: "FK_Filas_Modelos_ModeloId",
                        column: x => x.ModeloId,
                        principalTable: "Modelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    DivisionId = table.Column<int>(nullable: false),
                    GrupoId = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 32, nullable: true),
                    NombreCompleto = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empresas_Divisiones_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Empresas_Grupos_DivisionId_GrupoId",
                        columns: x => new { x.DivisionId, x.GrupoId },
                        principalTable: "Grupos",
                        principalColumns: new[] { "DivisionId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstadosFinancieros",
                columns: table => new
                {
                    Año = table.Column<int>(nullable: false),
                    Mes = table.Column<int>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: false),
                    ModeloId = table.Column<int>(nullable: false),
                    FilaId = table.Column<int>(nullable: false),
                    C1 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    C2 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    C3 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosFinancieros", x => new { x.Año, x.Mes, x.EmpresaId, x.ModeloId, x.FilaId });
                    table.UniqueConstraint("AK_EstadosFinancieros_Año_EmpresaId_FilaId_Mes_ModeloId", x => new { x.Año, x.EmpresaId, x.FilaId, x.Mes, x.ModeloId });
                    table.ForeignKey(
                        name: "FK_EstadosFinancieros_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstadosFinancieros_Modelos_ModeloId",
                        column: x => x.ModeloId,
                        principalTable: "Modelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstadosFinancieros_Filas_FilaId_ModeloId",
                        columns: x => new { x.FilaId, x.ModeloId },
                        principalTable: "Filas",
                        principalColumns: new[] { "Id", "ModeloId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Divisiones_Nombre",
                table: "Divisiones",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Nombre",
                table: "Empresas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_NombreCompleto",
                table: "Empresas",
                column: "NombreCompleto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_DivisionId_GrupoId",
                table: "Empresas",
                columns: new[] { "DivisionId", "GrupoId" });

            migrationBuilder.CreateIndex(
                name: "IX_EstadosFinancieros_EmpresaId",
                table: "EstadosFinancieros",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosFinancieros_ModeloId",
                table: "EstadosFinancieros",
                column: "ModeloId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosFinancieros_FilaId_ModeloId",
                table: "EstadosFinancieros",
                columns: new[] { "FilaId", "ModeloId" });

            migrationBuilder.CreateIndex(
                name: "IX_Filas_ModeloId",
                table: "Filas",
                column: "ModeloId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadosFinancieros");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Filas");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Modelos");

            migrationBuilder.DropTable(
                name: "Divisiones");
        }
    }
}
