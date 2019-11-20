using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alimatic.Nexus.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ColumnTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Name = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Securities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Securities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Name = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AccountId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    SecurityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Securities_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "Securities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    EnumValues = table.Column<string>(maxLength: 16384, nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    NotNull = table.Column<bool>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    TableId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Columns_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Columns_ColumnTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ColumnTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableRoles",
                columns: table => new
                {
                    TableId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    OverrideColumnsPermission = table.Column<bool>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false),
                    SecurityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableRoles", x => new { x.TableId, x.RoleId });
                    table.UniqueConstraint("AK_TableRoles_RoleId_TableId", x => new { x.RoleId, x.TableId });
                    table.ForeignKey(
                        name: "FK_TableRoles_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableRoles_Securities_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "Securities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableRoles_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    TableId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rows_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rows_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.UniqueConstraint("AK_UserRoles_RoleId_UserId", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ColumnRoles",
                columns: table => new
                {
                    ColumnId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnRoles", x => new { x.ColumnId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ColumnRoles_Columns_ColumnId",
                        column: x => x.ColumnId,
                        principalTable: "Columns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColumnRoles_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColumnRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RowColumns",
                columns: table => new
                {
                    RowId = table.Column<int>(nullable: false),
                    ColumnId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RowColumns", x => new { x.RowId, x.ColumnId });
                    table.UniqueConstraint("AK_RowColumns_ColumnId_RowId", x => new { x.ColumnId, x.RowId });
                    table.ForeignKey(
                        name: "FK_RowColumns_Columns_ColumnId",
                        column: x => x.ColumnId,
                        principalTable: "Columns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowColumns_Rows_RowId",
                        column: x => x.RowId,
                        principalTable: "Rows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Columns_TableId",
                table: "Columns",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_TypeId",
                table: "Columns",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_Name_TableId",
                table: "Columns",
                columns: new[] { "Name", "TableId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ColumnRoles_PermissionId",
                table: "ColumnRoles",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnRoles_RoleId",
                table: "ColumnRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnTypes_Name",
                table: "ColumnTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rows_TableId",
                table: "Rows",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Rows_UserId",
                table: "Rows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Securities_Name",
                table: "Securities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tables_Name",
                table: "Tables",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TableRoles_PermissionId",
                table: "TableRoles",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_TableRoles_SecurityId",
                table: "TableRoles",
                column: "SecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SecurityId",
                table: "Users",
                column: "SecurityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColumnRoles");

            migrationBuilder.DropTable(
                name: "RowColumns");

            migrationBuilder.DropTable(
                name: "TableRoles");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Columns");

            migrationBuilder.DropTable(
                name: "Rows");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ColumnTypes");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Securities");
        }
    }
}
