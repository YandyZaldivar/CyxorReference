using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cardyan.Accounting.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountClasification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountClasification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountEntryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountEntryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountNormalBalances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountNormalBalances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 126, nullable: true),
                    Description = table.Column<string>(maxLength: 126, nullable: false),
                    NormalBalanceId = table.Column<int>(nullable: false),
                    Balance = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    ClasificationId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountClasification_ClasificationId",
                        column: x => x.ClasificationId,
                        principalTable: "AccountClasification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountNormalBalances_NormalBalanceId",
                        column: x => x.NormalBalanceId,
                        principalTable: "AccountNormalBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "AccountType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountEntries",
                columns: table => new
                {
                    Amount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountEntries", x => new { x.AccountId, x.TransactionId });
                    table.ForeignKey(
                        name: "FK_AccountEntries_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountEntries_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountEntries_AccountEntryTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "AccountEntryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountClasification",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Real" },
                    { 2, "Nominal" }
                });

            migrationBuilder.InsertData(
                table: "AccountEntryTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Debit" },
                    { 2, "Credit" }
                });

            migrationBuilder.InsertData(
                table: "AccountNormalBalances",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Debit" },
                    { 2, "Credit" }
                });

            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Pasive" },
                    { 3, "Equity" },
                    { 4, "Revenus" },
                    { 5, "Incons" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountEntries_TransactionId",
                table: "AccountEntries",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountEntries_TypeId",
                table: "AccountEntries",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountEntryTypes_Name",
                table: "AccountEntryTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountNormalBalances_Name",
                table: "AccountNormalBalances",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ClasificationId",
                table: "Accounts",
                column: "ClasificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Code",
                table: "Accounts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_NormalBalanceId",
                table: "Accounts",
                column: "NormalBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentId",
                table: "Accounts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_TypeId",
                table: "Accounts",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountEntries");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AccountEntryTypes");

            migrationBuilder.DropTable(
                name: "AccountClasification");

            migrationBuilder.DropTable(
                name: "AccountNormalBalances");

            migrationBuilder.DropTable(
                name: "AccountType");
        }
    }
}
