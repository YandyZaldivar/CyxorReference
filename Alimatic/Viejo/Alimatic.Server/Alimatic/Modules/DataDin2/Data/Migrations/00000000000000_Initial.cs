using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alimatic.DataDin2.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Frequencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    DivisionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => new { x.DivisionId, x.Id });
                    table.ForeignKey(
                        name: "FK_Groups_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    RowCount = table.Column<int>(nullable: false),
                    ColumnCount = table.Column<int>(nullable: false),
                    FrequencyId = table.Column<int>(nullable: false),
                    IsEFModel = table.Column<bool>(nullable: false),
                    ColumnNames = table.Column<string>(maxLength: 1024, nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Frequencies_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "Frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enterprises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    DivisionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    FullName = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enterprises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enterprises_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enterprises_Groups_DivisionId_GroupId",
                        columns: x => new { x.DivisionId, x.GroupId },
                        principalTable: "Groups",
                        principalColumns: new[] { "DivisionId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rows", x => new { x.Id, x.ModelId });
                    table.ForeignKey(
                        name: "FK_Rows_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    Locked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => new { x.Year, x.Month, x.Day, x.ModelId });
                    table.UniqueConstraint("AK_Templates_Day_ModelId_Month_Year", x => new { x.Day, x.ModelId, x.Month, x.Year });
                    table.ForeignKey(
                        name: "FK_Templates_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: true),
                    EnterpriseId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Password = table.Column<string>(maxLength: 8192, nullable: true),
                    Email = table.Column<string>(maxLength: 64, nullable: true),
                    Permission = table.Column<int>(nullable: false),
                    SecurityLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    EnterpriseId = table.Column<int>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    RowId = table.Column<int>(nullable: false),
                    C01 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    C02 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    C03 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    C04 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    C05 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    C06 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    C07 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    C08 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    C09 = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => new { x.Year, x.Month, x.Day, x.EnterpriseId, x.ModelId, x.RowId });
                    table.UniqueConstraint("AK_Records_Day_EnterpriseId_ModelId_Month_RowId_Year", x => new { x.Day, x.EnterpriseId, x.ModelId, x.Month, x.RowId, x.Year });
                    table.ForeignKey(
                        name: "FK_Records_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Records_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Records_Rows_RowId_ModelId",
                        columns: x => new { x.RowId, x.ModelId },
                        principalTable: "Rows",
                        principalColumns: new[] { "Id", "ModelId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Records_Templates_Year_Month_Day_ModelId",
                        columns: x => new { x.Year, x.Month, x.Day, x.ModelId },
                        principalTable: "Templates",
                        principalColumns: new[] { "Year", "Month", "Day", "ModelId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserModels",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModels", x => new { x.UserId, x.ModelId });
                    table.UniqueConstraint("AK_UserModels_ModelId_UserId", x => new { x.ModelId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserModels_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserModels_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.UniqueConstraint("AK_UserRole_RoleId_UserId", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_Name",
                table: "Divisions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enterprises_FullName",
                table: "Enterprises",
                column: "FullName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enterprises_Name",
                table: "Enterprises",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enterprises_DivisionId_GroupId",
                table: "Enterprises",
                columns: new[] { "DivisionId", "GroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_Models_FrequencyId",
                table: "Models",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_EnterpriseId",
                table: "Records",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_ModelId",
                table: "Records",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_RowId_ModelId",
                table: "Records",
                columns: new[] { "RowId", "ModelId" });

            migrationBuilder.CreateIndex(
                name: "IX_Records_Year_Month_Day_ModelId",
                table: "Records",
                columns: new[] { "Year", "Month", "Day", "ModelId" });

            migrationBuilder.CreateIndex(
                name: "IX_Rows_ModelId",
                table: "Rows",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_ModelId",
                table: "Templates",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EnterpriseId",
                table: "Users",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "UserModels");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Rows");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Enterprises");

            migrationBuilder.DropTable(
                name: "Frequencies");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Divisions");
        }
    }
}
