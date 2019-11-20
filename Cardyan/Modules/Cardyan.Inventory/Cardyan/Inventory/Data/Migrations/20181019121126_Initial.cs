using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cardyan.Inventory.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Associates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 126, nullable: true),
                    Name = table.Column<string>(maxLength: 126, nullable: false),
                    LastName = table.Column<string>(maxLength: 126, nullable: true),
                    Description = table.Column<string>(maxLength: 16380, nullable: true),
                    Email = table.Column<string>(maxLength: 126, nullable: true),
                    Phone = table.Column<string>(maxLength: 24, nullable: true),
                    Phone2 = table.Column<string>(maxLength: 24, nullable: true),
                    Address = table.Column<string>(maxLength: 126, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 126, nullable: false),
                    Name = table.Column<string>(maxLength: 126, nullable: true),
                    Description = table.Column<string>(maxLength: 16380, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 126, nullable: false),
                    Description = table.Column<string>(maxLength: 16380, nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 126, nullable: false),
                    Description = table.Column<string>(maxLength: 16380, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LastPrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    AveragePrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    MinimumPrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    MaximumPrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    InCount = table.Column<int>(nullable: false),
                    OutCount = table.Column<int>(nullable: false),
                    InAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    FifoOutAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    LifoOutAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 126, nullable: false),
                    Description = table.Column<string>(maxLength: 16380, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Valuations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valuations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseProductStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LastPrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    AveragePrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    MinimumPrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    MaximumPrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    InCount = table.Column<int>(nullable: false),
                    OutCount = table.Column<int>(nullable: false),
                    InAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    FifoOutAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    LifoOutAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseProductStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LastPrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    AveragePrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    MinimumPrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    MaximumPrice = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    InCount = table.Column<int>(nullable: false),
                    OutCount = table.Column<int>(nullable: false),
                    InAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    FifoOutAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    LifoOutAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 126, nullable: false),
                    Name = table.Column<string>(maxLength: 126, nullable: true),
                    Description = table.Column<string>(maxLength: 16380, nullable: true),
                    MeasurementUnitId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    StatisticId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductStatistics_StatisticId",
                        column: x => x.StatisticId,
                        principalTable: "ProductStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 126, nullable: false),
                    Description = table.Column<string>(maxLength: 16380, nullable: true),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_PropertyTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssociateTags",
                columns: table => new
                {
                    AssociateId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateTags", x => new { x.AssociateId, x.TagId });
                    table.ForeignKey(
                        name: "FK_AssociateTags_Associates_AssociateId",
                        column: x => x.AssociateId,
                        principalTable: "Associates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssociateTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchTags",
                columns: table => new
                {
                    BranchId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchTags", x => new { x.BranchId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BranchTags_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 126, nullable: false),
                    Name = table.Column<string>(maxLength: 126, nullable: true),
                    Description = table.Column<string>(maxLength: 16380, nullable: true),
                    BranchId = table.Column<int>(nullable: false),
                    ValuationId = table.Column<int>(nullable: false),
                    StatisticId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Warehouses_WarehouseStatistics_StatisticId",
                        column: x => x.StatisticId,
                        principalTable: "WarehouseStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Warehouses_Valuations_ValuationId",
                        column: x => x.ValuationId,
                        principalTable: "Valuations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 126, nullable: true),
                    Description = table.Column<string>(maxLength: 16380, nullable: true),
                    Header = table.Column<string>(maxLength: 126, nullable: true),
                    Data = table.Column<byte[]>(maxLength: 262144, nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => new { x.ProductId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductProperties",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    PropertyId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 16380, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProperties", x => new { x.ProductId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ProductProperties_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 126, nullable: true),
                    Description = table.Column<string>(maxLength: 16380, nullable: true),
                    WarehouseId = table.Column<int>(nullable: false),
                    LinkedWarehouseId = table.Column<int>(nullable: true),
                    LinkedMovementId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    AssociateId = table.Column<int>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movements_Associates_AssociateId",
                        column: x => x.AssociateId,
                        principalTable: "Associates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movements_Movements_LinkedMovementId",
                        column: x => x.LinkedMovementId,
                        principalTable: "Movements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movements_Warehouses_LinkedWarehouseId",
                        column: x => x.LinkedWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movements_MovementTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MovementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movements_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseLocations",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 126, nullable: false),
                    Name = table.Column<string>(maxLength: 126, nullable: true),
                    Description = table.Column<string>(maxLength: 16380, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseLocations", x => new { x.WarehouseId, x.LocationId });
                    table.UniqueConstraint("AK_WarehouseLocations_LocationId_WarehouseId", x => new { x.LocationId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_WarehouseLocations_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseProductTags",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseProductTags", x => new { x.WarehouseId, x.ProductId, x.TagId });
                    table.UniqueConstraint("AK_WarehouseProductTags_ProductId_TagId_WarehouseId", x => new { x.ProductId, x.TagId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_WarehouseProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseProductTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseProductTags_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTags",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTags", x => new { x.WarehouseId, x.TagId });
                    table.UniqueConstraint("AK_WarehouseTags_TagId_WarehouseId", x => new { x.TagId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_WarehouseTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseTags_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FifoExistences",
                columns: table => new
                {
                    MovementId = table.Column<int>(nullable: false),
                    WarehouseId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    Count = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FifoExistences", x => new { x.MovementId, x.WarehouseId, x.ProductId });
                    table.UniqueConstraint("AK_FifoExistences_MovementId_ProductId_WarehouseId", x => new { x.MovementId, x.ProductId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_FifoExistences_Movements_MovementId",
                        column: x => x.MovementId,
                        principalTable: "Movements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FifoExistences_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FifoExistences_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LifoExistences",
                columns: table => new
                {
                    MovementId = table.Column<int>(nullable: false),
                    WarehouseId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "Decimal(22, 2)", nullable: false),
                    Count = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifoExistences", x => new { x.MovementId, x.WarehouseId, x.ProductId });
                    table.UniqueConstraint("AK_LifoExistences_MovementId_ProductId_WarehouseId", x => new { x.MovementId, x.ProductId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_LifoExistences_Movements_MovementId",
                        column: x => x.MovementId,
                        principalTable: "Movements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LifoExistences_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LifoExistences_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovementProducts",
                columns: table => new
                {
                    MovementId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    FifoAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    LifoAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true),
                    AverageAmount = table.Column<decimal>(type: "Decimal(22, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementProducts", x => new { x.MovementId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_MovementProducts_Movements_MovementId",
                        column: x => x.MovementId,
                        principalTable: "Movements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovementProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovementTags",
                columns: table => new
                {
                    MovementId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementTags", x => new { x.MovementId, x.TagId });
                    table.ForeignKey(
                        name: "FK_MovementTags_Movements_MovementId",
                        column: x => x.MovementId,
                        principalTable: "Movements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovementTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseLocationTags",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseLocationTags", x => new { x.WarehouseId, x.LocationId, x.TagId });
                    table.UniqueConstraint("AK_WarehouseLocationTags_LocationId_TagId_WarehouseId", x => new { x.LocationId, x.TagId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_WarehouseLocationTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseLocationTags_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseLocationTags_WarehouseLocations_WarehouseId_Locatio~",
                        columns: x => new { x.WarehouseId, x.LocationId },
                        principalTable: "WarehouseLocations",
                        principalColumns: new[] { "WarehouseId", "LocationId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseProducts",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 16380, nullable: true),
                    Minimum = table.Column<int>(nullable: true),
                    Maximum = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    StatisticId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseProducts", x => new { x.WarehouseId, x.ProductId });
                    table.UniqueConstraint("AK_WarehouseProducts_ProductId_WarehouseId", x => new { x.ProductId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_WarehouseProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseProducts_WarehouseProductStatistics_StatisticId",
                        column: x => x.StatisticId,
                        principalTable: "WarehouseProductStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseProducts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseProducts_WarehouseLocations_WarehouseId_LocationId",
                        columns: x => new { x.WarehouseId, x.LocationId },
                        principalTable: "WarehouseLocations",
                        principalColumns: new[] { "WarehouseId", "LocationId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MovementTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "In" },
                    { 2, "Out" }
                });

            migrationBuilder.InsertData(
                table: "PropertyTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 11, "UInt64" },
                    { 9, "UInt32" },
                    { 6, "UInt16" },
                    { 25, "TimeSpan" },
                    { 23, "Time" },
                    { 20, "Text" },
                    { 19, "StringEnum" },
                    { 18, "String" },
                    { 12, "Single" },
                    { 4, "SByte" },
                    { 21, "LongText" },
                    { 8, "Int64Enum" },
                    { 10, "Int64" },
                    { 5, "Int16" },
                    { 15, "Guid" },
                    { 17, "File" },
                    { 13, "Double" },
                    { 14, "Decimal" },
                    { 26, "DateTimeOffset" },
                    { 24, "DateTime" },
                    { 22, "Date" },
                    { 2, "Char" },
                    { 3, "Byte" },
                    { 1, "Boolean" },
                    { 16, "Binary" },
                    { 7, "Int32" }
                });

            migrationBuilder.InsertData(
                table: "Valuations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "Fifo" },
                    { 1, "Average" },
                    { 3, "Lifo" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Associates_Name",
                table: "Associates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssociateTags_TagId",
                table: "AssociateTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Code",
                table: "Branches",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Name",
                table: "Branches",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BranchTags_TagId",
                table: "BranchTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FifoExistences_ProductId",
                table: "FifoExistences",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FifoExistences_WarehouseId",
                table: "FifoExistences",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LifoExistences_ProductId",
                table: "LifoExistences",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LifoExistences_WarehouseId",
                table: "LifoExistences",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementUnits_Name",
                table: "MeasurementUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovementProducts_ProductId",
                table: "MovementProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_AssociateId",
                table: "Movements",
                column: "AssociateId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_LinkedMovementId",
                table: "Movements",
                column: "LinkedMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_LinkedWarehouseId",
                table: "Movements",
                column: "LinkedWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_TypeId",
                table: "Movements",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_WarehouseId",
                table: "Movements",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementTags_TagId",
                table: "MovementTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementTypes_Name",
                table: "MovementTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductProperties_PropertyId",
                table: "ProductProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_MeasurementUnitId",
                table: "Products",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StatisticId",
                table: "Products",
                column: "StatisticId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagId",
                table: "ProductTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Name",
                table: "Properties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_TypeId",
                table: "Properties",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTypes_Name",
                table: "PropertyTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Valuations_Name",
                table: "Valuations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLocations_Code",
                table: "WarehouseLocations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLocations_WarehouseId_Name",
                table: "WarehouseLocations",
                columns: new[] { "WarehouseId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLocationTags_TagId",
                table: "WarehouseLocationTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_CategoryId",
                table: "WarehouseProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_StatisticId",
                table: "WarehouseProducts",
                column: "StatisticId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_WarehouseId_LocationId",
                table: "WarehouseProducts",
                columns: new[] { "WarehouseId", "LocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProductTags_TagId",
                table: "WarehouseProductTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Code",
                table: "Warehouses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_StatisticId",
                table: "Warehouses",
                column: "StatisticId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_ValuationId",
                table: "Warehouses",
                column: "ValuationId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_BranchId_Name",
                table: "Warehouses",
                columns: new[] { "BranchId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociateTags");

            migrationBuilder.DropTable(
                name: "BranchTags");

            migrationBuilder.DropTable(
                name: "FifoExistences");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "LifoExistences");

            migrationBuilder.DropTable(
                name: "MovementProducts");

            migrationBuilder.DropTable(
                name: "MovementTags");

            migrationBuilder.DropTable(
                name: "ProductProperties");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "WarehouseLocationTags");

            migrationBuilder.DropTable(
                name: "WarehouseProducts");

            migrationBuilder.DropTable(
                name: "WarehouseProductTags");

            migrationBuilder.DropTable(
                name: "WarehouseTags");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "WarehouseProductStatistics");

            migrationBuilder.DropTable(
                name: "WarehouseLocations");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Associates");

            migrationBuilder.DropTable(
                name: "MovementTypes");

            migrationBuilder.DropTable(
                name: "PropertyTypes");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "MeasurementUnits");

            migrationBuilder.DropTable(
                name: "ProductStatistics");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "WarehouseStatistics");

            migrationBuilder.DropTable(
                name: "Valuations");
        }
    }
}
