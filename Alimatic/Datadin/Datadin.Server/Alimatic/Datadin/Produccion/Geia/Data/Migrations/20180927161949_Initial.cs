using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alimatic.Datadin.Produccion.Geia.Data.Migrations
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
                values: new object[,]
                {
                    { 1, "Agroalimentaria" },
                    { 2, "Alimentaria" },
                    { 3, "Pesca" },
                    { 4, "Servicios" }
                });

            migrationBuilder.InsertData(
                table: "Frequencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Daily" },
                    { 2, "Monthly" }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "DivisionId", "Id", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Carnes" },
                    { 1, 2, "Lácteos" },
                    { 1, 3, "Otras" },
                    { 2, 1, "Bebidas" },
                    { 2, 2, "Cervezas" },
                    { 2, 3, "Aceites" },
                    { 2, 4, "Otras" },
                    { 3, 1, "Plataforma" },
                    { 3, 2, "Acuicultura" },
                    { 3, 3, "Otras" },
                    { 4, 1, "Servicios" }
                });

            migrationBuilder.InsertData(
                table: "Enterprises",
                columns: new[] { "Id", "DivisionId", "FullName", "GroupId", "Name" },
                values: new object[,]
                {
                    { 1580, 1, "Empresa cárnica Pinar del Río", 1, "Cárnica Pinar del Río" },
                    { 7582, 3, "Empresa Pesquera de Santiago de Cuba PESCASAN", 2, "PESCASAN" },
                    { 7541, 3, "Empresa Pesquera de Matanzas PESCAMAT", 2, "PESCAMAT" },
                    { 7491, 3, "Empresa Pesquera de Holguín PESCAHOL", 2, "PESCAHOL" },
                    { 7462, 3, "Empresa Pesquera de Ciego de Ávila PESCAVILA", 2, "PESCAVILA" },
                    { 7430, 3, "Empresa Pesquera de Pinar del Río PESCARIO", 2, "PESCARIO" },
                    { 12540, 3, "Empresa Pesquera Industrial de Cienfuegos EPICIEN", 1, "EPICIEN" },
                    { 12539, 3, "Empresa Pesquera Industrial de Granma EPIGRAM", 1, "EPIGRAM" },
                    { 12538, 3, "Empresa Pesquera Industrial de Ciego de Ávila EPIVILA", 1, "EPIVILA" },
                    { 12537, 3, "Empresa Pesquera Industrial de Santa Cruz del Sur EPISUR", 1, "EPISUR" },
                    { 7597, 3, "Empresa Pesquera de Villa Clara PESCAVILLA", 2, "PESCAVILLA" },
                    { 12536, 3, "Empresa Pesquera Industrial de Sancti Spíritus EPISAN", 1, "EPISAN" },
                    { 12534, 3, "Empresa Pesquera Industrial de La Coloma EPICOL", 1, "EPICOL" },
                    { 7969, 3, "Empresa Lanchera Flota del Golfo FLOGOLFO", 1, "FLOGOLFO" },
                    { 7892, 3, "Empresa Pesquera Industrial Isla de la Juventud PESCAISLA", 1, "PESCAISLA" },
                    { 7508, 3, "Pesquera Industrial de Batabanó PESCAHABANA", 1, "PESCAHABANA" },
                    { 11989, 2, "Empresa cubana del pan", 4, "Cubana del Pan" },
                    { 14194, 2, "Empresa de confitería y derivados de la harina", 4, "Confitera" },
                    { 1734, 2, "Empresa cubana de molinería", 4, "Molinera" },
                    { 13819, 2, "Empresa procesadora de soya (PDS)", 3, "PDS" },
                    { 2794, 2, "Empresa refinadora de aceites de Santiago de Cuba (Erasol)", 3, "Erasol" },
                    { 12535, 3, "Empresa Pesquera Industrial de Caibarién EPICAI", 1, "EPICAI" },
                    { 7622, 3, "Empresa Pesquera de Sancti Spíritus PESCASPIR", 2, "PESCASPIR" },
                    { 7649, 3, "Empresa Pesquera de Camagüey PESCACAM", 2, "PESCACAM" },
                    { 7881, 3, "Empresa Pesquera de La Habana ACUABANA", 2, "ACUABANA" },
                    { 13541, 4, "Empresa Importadora Exportadora del Minal ALIMPEX", 1, "ALIMPEX" },
                    { 2046, 4, "Empresa Revista Mar y Pesca", 1, "Revista Mar y Pesca" },
                    { 6301, 4, "Empresa de Servicios Varios del Minal EMSERVA", 1, "EMSERVA" },
                    { 7730, 4, "Empresa de Construcciones Metálicas y Eléctricas COMELEC", 1, "COMELEC" },
                    { 7744, 4, "Empresa de Refrigeración y Calderas del Minal SERIC", 1, "SERIC" },
                    { 6228, 4, "Empresa de Sistemas Automatizados ALIMATIC", 1, "ALIMATIC" },
                    { 11721, 4, "Empresa de Servicios de Seguridad y Protección ESEP", 1, "ESEP" },
                    { 12070, 4, "Empresa de Diseño y Servicios de Ingeniería IDS", 1, "IDS" },
                    { 6223, 4, "Proyectos de Construcciones y Servicios Navales CEPRONA", 1, "CEPRONA" },
                    { 7536, 3, "Empresa de Transporte Refrigerado ATLAS", 3, "ATLAS" },
                    { 7980, 3, "Empresa Terminal Refrigerada TERREF", 3, "TERREF" },
                    { 11163, 3, "Empresa Productora de Alimentos de Regla PRODAL", 3, "PRODAL" },
                    { 11307, 3, "Empresa Comercial de Alimentos COPMAR", 3, "COPMAR" },
                    { 7955, 3, "Empresa Proveedora del Minal PROPES", 3, "PROPES" },
                    { 4168, 3, "Empresa Pesca Caribe", 3, "Pesca Caribe" },
                    { 13051, 3, "Empresa Comercial CARIBEX", 3, "CARIBEX" },
                    { 14202, 3, "Empresa para el cultivo del Camarón GDECAN", 3, "GDECAN" },
                    { 14699, 3, "Empresa Pesquera de Guantánamo PESCAGUÁN", 2, "PESCAGUAN" },
                    { 14700, 3, "Empresa Pesquera de Las Tunas PESCATUN", 2, "PESCATUN" },
                    { 7911, 3, "Empresa Pesquera de Granma PESCAGRAN", 2, "PESCAGRAN" },
                    { 7888, 3, "Preparación Acuícola de Mampostón EDTA", 2, "EDTA" },
                    { 13480, 2, "Empresa de aceites y grasas comestibles Camagüey", 3, "Aceites Camagüey" },
                    { 14097, 4, "Grupo Empresarial de la Industria Alimentaria", 1, "GEIA" },
                    { 1617, 2, "Empresa de aceites y grasas comestibles La Habana", 3, "Aceites La Habana" },
                    { 4175, 2, "Empresa cervecería Santiago de Cuba (Hatuey)", 2, "Cervecería Hatuey" },
                    { 1598, 1, "Empresa complejo lácteo Habana", 2, "Lácteos La Habana" },
                    { 1597, 1, "Empresa de aseguramiento de la industria láctea", 2, "Lácteos Aseguramiento" },
                    { 1595, 1, "Empresa de productos lácteos Artemisa", 2, "Lácteos Artemisa" },
                    { 1594, 1, "Empresa de productos lácteos y confitería Pinar del Río", 2, "Lácteos Pinar del Río" },
                    { 1384, 1, "Empresa de productos lácteos Coppelia", 2, "Lácteos Coppelia" },
                    { 14203, 1, "Empresa de aseguramiento de la industria cárnica", 1, "ASECAR" },
                    { 13017, 1, "Empresa cárnica Nueva Paz", 1, "Cárnica Nueva Paz" },
                    { 12700, 1, "Empresa cárnica Tauro", 1, "Cárnica Tauro" },
                    { 2756, 1, "Empresa cárnica Cienfuegos", 1, "Cárnica Cienfuegos" },
                    { 1599, 1, "Empresa de productos lácteos Matanzas", 2, "Lácteos Matanzas" },
                    { 2664, 1, "Empresa cárnica Habana", 1, "Cárnica La Habana" },
                    { 1961, 1, "Empresa cárnica Ciego de Ávila", 1, "Cárnica Ciego de Ávila" },
                    { 1954, 1, "Empresa cárnica Guantánamo", 1, "Cárnica Guantánamo" },
                    { 1593, 1, "Empresa cárnica Santiago", 1, "Cárnica Santiago de Cuba" },
                    { 1592, 1, "Empresa cárnica Granma", 1, "Cárnica Granma" },
                    { 1591, 1, "Empresa cárnica Holguín", 1, "Cárnica Holguín" },
                    { 1589, 1, "Empresa cárnica Camagüey", 1, "Cárnica Camagüey" },
                    { 1588, 1, "Empresa cárnica Sancti Spíritus", 1, "Cárnica Sancti Spíritus" },
                    { 1587, 1, "Empresa cárnica Villa Clara", 1, "Cárnica Villa Clara" },
                    { 1585, 1, "Empresa cárnica Matanzas", 1, "Cárnica Matanzas" },
                    { 2660, 1, "Empresa cárnica Las Tunas", 1, "Cárnica Las Tunas" },
                    { 1600, 1, "Empresa de productos lácteos Villa Clara", 2, "Lácteos Villa Clara" },
                    { 1601, 1, "Empresa de productos lácteos Escambray", 2, "Lácteos Escambray" },
                    { 1606, 1, "Empresa de productos lácteos Río Zaza", 2, "Lácteos Río Zaza" },
                    { 3008, 2, "Empresa cervecería Guido Pérez (Modelo)", 2, "Cervecería Modelo" },
                    { 1672, 2, "Empresa cervecería Antonio Díaz Santana (Manacas)", 2, "Cervecería Manacas" },
                    { 13042, 2, "Empresa aseguramiento industrias bebidas y refrescos", 1, "EMBER Aseguramiento" },
                    { 4796, 2, "Empresa de bebidas y refrescos Mayabeque", 1, "EMBER Mayabeque" },
                    { 1679, 2, "Empresa de bebidas y refrescos Ciego de Ávila", 1, "EMBER Ciego de Ávila" },
                    { 1674, 2, "Empresa de bebidas y refrescos Pinar del Río", 1, "EMBER Pinar del Río" },
                    { 1668, 2, "Empresa de bebidas y refrescos Santiago de Cuba", 1, "EMBER Santiago de Cuba" },
                    { 1667, 2, "Empresa de bebidas y refrescos Granma", 1, "EMBER Granma" },
                    { 1666, 2, "Empresa de bebidas y refrescos Camagüey", 1, "EMBER Camagüey" },
                    { 1665, 2, "Empresa de bebidas y refrescos Villa Clara", 1, "EMBER Villa Clara" },
                    { 1663, 2, "Empresa de bebidas y refrescos La Habana", 1, "EMBER La Habana" },
                    { 4272, 1, "Empresa productora de alimentos Isla de la Juventud", 3, "Alimentos Isla" },
                    { 14216, 1, "Empresa de torrefacción y comercialización de café", 3, "Torrefactora de Café" },
                    { 14215, 1, "Empresa de conservas de vegetales", 3, "Conservas de Vegetales" },
                    { 1616, 1, "Empresa de productos lácteos Guantánamo", 2, "Lácteos Guantánamo" },
                    { 1615, 1, "Empresa de productos lácteos Santiago de Cuba", 2, "Lácteos Santiago de Cuba" },
                    { 1612, 1, "Empresa de productos lácteos Bayamo", 2, "Lácteos Bayamo" },
                    { 1611, 1, "Empresa de productos lácteos Holguín", 2, "Lácteos Holguín" },
                    { 1610, 1, "Empresa de productos lácteos Las Tunas", 2, "Lácteos Las Tunas" },
                    { 1609, 1, "Empresa de productos lácteos Camagüey", 2, "Lácteos Camagüey" },
                    { 1607, 1, "Empresa de productos lácteos Ciego de Ávila", 2, "Lácteos Ciego de Ávila" },
                    { 11724, 2, "Empresa cervecería Tinima", 2, "Cervecería Tinima" },
                    { 14091, 4, "OSDE Grupo Empresarial de la Industria Alimentaria", 1, "OSDE GEIA" }
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
