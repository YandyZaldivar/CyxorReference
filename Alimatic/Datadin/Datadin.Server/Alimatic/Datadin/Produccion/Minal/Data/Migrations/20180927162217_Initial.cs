using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alimatic.Datadin.Produccion.Minal.Data.Migrations
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

            migrationBuilder.InsertData(
                table: "Divisions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Minal" });

            migrationBuilder.InsertData(
                table: "Frequencies",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Daily" });

            migrationBuilder.InsertData(
                table: "Frequencies",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Monthly" });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "DivisionId", "Id", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Coralsa" },
                    { 1, 2, "Cubaron" },
                    { 1, 3, "Empleadoras" },
                    { 1, 4, "Presupuestadas" }
                });

            migrationBuilder.InsertData(
                table: "Enterprises",
                columns: new[] { "Id", "DivisionId", "FullName", "GroupId", "Name" },
                values: new object[,]
                {
                    { 100001, 1, "Empresa Mixta Cervecería Bucanero S.A.", 1, "Bucanero" },
                    { 400004, 1, "Instituto Marítimo Pesquero Andres Gonzalez Lines", 4, "Gonzalez Lines" },
                    { 400003, 1, "Centro de Investigaciones Pesqueras", 4, "CIP" },
                    { 400002, 1, "Oficina Nacional de Inspección Estatal", 4, "ONIE" },
                    { 400001, 1, "Instituto de Investigaciones de la Industria Alimentaria", 4, "IIIA" },
                    { 300001, 1, "Empleadora S.A.", 3, "Empleadora" },
                    { 200003, 1, "Havana Club International S.A.", 2, "Habana Club" },
                    { 200002, 1, "Empresa de Alimentos y Bebidas la Estancia S.A.", 2, "La Estancia" },
                    { 200001, 1, "Las Lomas S.A.", 2, "Las Lomas" },
                    { 100010, 1, "Papas & Company S.A.", 1, "Papas & Company" },
                    { 100009, 1, "Nescor S.A.", 1, "Nescor" },
                    { 100008, 1, "Lefersa S.A.", 1, "Lefersa" },
                    { 100007, 1, "Stella S.A.", 1, "Stella" },
                    { 100006, 1, "Industrial Molinera de la Habana S.A.", 1, "Molinera" },
                    { 100005, 1, "Los Portales S.A.", 1, "Los Portales" },
                    { 100004, 1, "Coralac S.A.", 1, "Coralac" },
                    { 100003, 1, "Coracán S.A.", 1, "Coracán" },
                    { 100002, 1, "Bravo S.A.", 1, "Bravo" },
                    { 400005, 1, "Unidad Presupuestada de Administración Interna", 4, "UPPD" },
                    { 400006, 1, "UP Organismo Central", 4, "UP Organismo Central" }
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
