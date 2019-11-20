﻿// <auto-generated />
using System;
using Cardyan.Inventory.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cardyan.Inventory.Data.Migrations
{
    [DbContext(typeof(CardyanDbContext))]
    partial class CardyanDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Cardyan.Inventory.Models.Associate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasMaxLength(126);

                    b.Property<string>("Code")
                        .HasMaxLength(126);

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<string>("Email")
                        .HasMaxLength(126);

                    b.Property<string>("LastName")
                        .HasMaxLength(126);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.Property<string>("Phone")
                        .HasMaxLength(24);

                    b.Property<string>("Phone2")
                        .HasMaxLength(24);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Associates");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.AssociateTag", b =>
                {
                    b.Property<int>("AssociateId");

                    b.Property<int>("TagId");

                    b.HasKey("AssociateId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("AssociateTags");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<string>("Name")
                        .HasMaxLength(126);

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.BranchTag", b =>
                {
                    b.Property<int>("BranchId");

                    b.Property<int>("TagId");

                    b.HasKey("BranchId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("BranchTags");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.Property<int?>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.FifoExistence", b =>
                {
                    b.Property<int>("MovementId");

                    b.Property<int>("WarehouseId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Count");

                    b.Property<DateTime>("DateTime");

                    b.Property<decimal>("Price")
                        .HasColumnType("Decimal(22, 2)");

                    b.HasKey("MovementId", "WarehouseId", "ProductId");

                    b.HasAlternateKey("MovementId", "ProductId", "WarehouseId");

                    b.HasIndex("ProductId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("FifoExistences");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasMaxLength(262144);

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<string>("Header")
                        .HasMaxLength(126);

                    b.Property<string>("Name")
                        .HasMaxLength(126);

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.LifoExistence", b =>
                {
                    b.Property<int>("MovementId");

                    b.Property<int>("WarehouseId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Count");

                    b.Property<DateTime>("DateTime");

                    b.Property<decimal>("Price")
                        .HasColumnType("Decimal(22, 2)");

                    b.HasKey("MovementId", "WarehouseId", "ProductId");

                    b.HasAlternateKey("MovementId", "ProductId", "WarehouseId");

                    b.HasIndex("ProductId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("LifoExistences");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.MeasurementUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MeasurementUnits");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Movement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AssociateId");

                    b.Property<string>("Code")
                        .HasMaxLength(126);

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<int?>("LinkedMovementId");

                    b.Property<int?>("LinkedWarehouseId");

                    b.Property<int>("TypeId");

                    b.Property<int>("WarehouseId");

                    b.HasKey("Id");

                    b.HasIndex("AssociateId");

                    b.HasIndex("LinkedMovementId");

                    b.HasIndex("LinkedWarehouseId");

                    b.HasIndex("TypeId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Movements");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.MovementProduct", b =>
                {
                    b.Property<int>("MovementId");

                    b.Property<int>("ProductId");

                    b.Property<decimal?>("AverageAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<int>("Count");

                    b.Property<decimal?>("FifoAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("LifoAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("Decimal(22, 2)");

                    b.HasKey("MovementId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("MovementProducts");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.MovementTag", b =>
                {
                    b.Property<int>("MovementId");

                    b.Property<int>("TagId");

                    b.HasKey("MovementId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("MovementTags");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.MovementType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(4);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MovementTypes");

                    b.HasData(
                        new { Id = 1, Name = "In" },
                        new { Id = 2, Name = "Out" }
                    );
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CategoryId");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<int>("MeasurementUnitId");

                    b.Property<string>("Name")
                        .HasMaxLength(126);

                    b.Property<int>("StatisticId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("MeasurementUnitId");

                    b.HasIndex("StatisticId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.ProductProperty", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("PropertyId");

                    b.Property<string>("Value")
                        .HasMaxLength(16380);

                    b.HasKey("ProductId", "PropertyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("ProductProperties");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.ProductStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AveragePrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("FifoOutAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("InAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<int>("InCount");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("LifoOutAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("MaximumPrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("MinimumPrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<int>("OutCount");

                    b.HasKey("Id");

                    b.ToTable("ProductStatistics");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.ProductTag", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("TagId");

                    b.HasKey("ProductId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ProductTags");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Property", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("TypeId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.PropertyType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PropertyTypes");

                    b.HasData(
                        new { Id = 16, Name = "Binary" },
                        new { Id = 1, Name = "Boolean" },
                        new { Id = 3, Name = "Byte" },
                        new { Id = 2, Name = "Char" },
                        new { Id = 22, Name = "Date" },
                        new { Id = 24, Name = "DateTime" },
                        new { Id = 26, Name = "DateTimeOffset" },
                        new { Id = 14, Name = "Decimal" },
                        new { Id = 13, Name = "Double" },
                        new { Id = 17, Name = "File" },
                        new { Id = 15, Name = "Guid" },
                        new { Id = 5, Name = "Int16" },
                        new { Id = 7, Name = "Int32" },
                        new { Id = 10, Name = "Int64" },
                        new { Id = 8, Name = "Int64Enum" },
                        new { Id = 21, Name = "LongText" },
                        new { Id = 4, Name = "SByte" },
                        new { Id = 12, Name = "Single" },
                        new { Id = 18, Name = "String" },
                        new { Id = 19, Name = "StringEnum" },
                        new { Id = 20, Name = "Text" },
                        new { Id = 23, Name = "Time" },
                        new { Id = 25, Name = "TimeSpan" },
                        new { Id = 6, Name = "UInt16" },
                        new { Id = 9, Name = "UInt32" },
                        new { Id = 11, Name = "UInt64" }
                    );
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Valuation", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Valuations");

                    b.HasData(
                        new { Id = 1, Name = "Average" },
                        new { Id = 2, Name = "Fifo" },
                        new { Id = 3, Name = "Lifo" }
                    );
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BranchId");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<string>("Name")
                        .HasMaxLength(126);

                    b.Property<int>("StatisticId");

                    b.Property<int>("ValuationId");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("StatisticId");

                    b.HasIndex("ValuationId");

                    b.HasIndex("BranchId", "Name")
                        .IsUnique();

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseLocation", b =>
                {
                    b.Property<int>("WarehouseId");

                    b.Property<int>("LocationId");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<string>("Name")
                        .HasMaxLength(126);

                    b.HasKey("WarehouseId", "LocationId");

                    b.HasAlternateKey("LocationId", "WarehouseId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("WarehouseId", "Name")
                        .IsUnique();

                    b.ToTable("WarehouseLocations");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseLocationTag", b =>
                {
                    b.Property<int>("WarehouseId");

                    b.Property<int>("LocationId");

                    b.Property<int>("TagId");

                    b.HasKey("WarehouseId", "LocationId", "TagId");

                    b.HasAlternateKey("LocationId", "TagId", "WarehouseId");

                    b.HasIndex("TagId");

                    b.ToTable("WarehouseLocationTags");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseProduct", b =>
                {
                    b.Property<int>("WarehouseId");

                    b.Property<int>("ProductId");

                    b.Property<int?>("CategoryId");

                    b.Property<string>("Description")
                        .HasMaxLength(16380);

                    b.Property<int?>("LocationId");

                    b.Property<int?>("Maximum");

                    b.Property<int?>("Minimum");

                    b.Property<int>("StatisticId");

                    b.HasKey("WarehouseId", "ProductId");

                    b.HasAlternateKey("ProductId", "WarehouseId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StatisticId");

                    b.HasIndex("WarehouseId", "LocationId");

                    b.ToTable("WarehouseProducts");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseProductStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AveragePrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("FifoOutAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("InAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<int>("InCount");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("LifoOutAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("MaximumPrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("MinimumPrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<int>("OutCount");

                    b.HasKey("Id");

                    b.ToTable("WarehouseProductStatistics");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseProductTag", b =>
                {
                    b.Property<int>("WarehouseId");

                    b.Property<int>("ProductId");

                    b.Property<int>("TagId");

                    b.HasKey("WarehouseId", "ProductId", "TagId");

                    b.HasAlternateKey("ProductId", "TagId", "WarehouseId");

                    b.HasIndex("TagId");

                    b.ToTable("WarehouseProductTags");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AveragePrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("FifoOutAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("InAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<int>("InCount");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("LifoOutAmount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("MaximumPrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal>("MinimumPrice")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<int>("OutCount");

                    b.HasKey("Id");

                    b.ToTable("WarehouseStatistics");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseTag", b =>
                {
                    b.Property<int>("WarehouseId");

                    b.Property<int>("TagId");

                    b.HasKey("WarehouseId", "TagId");

                    b.HasAlternateKey("TagId", "WarehouseId");

                    b.ToTable("WarehouseTags");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.AssociateTag", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Associate", "Associate")
                        .WithMany("Tags")
                        .HasForeignKey("AssociateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Tag", "Tag")
                        .WithMany("Associates")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.BranchTag", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Branch", "Branch")
                        .WithMany("Tags")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Tag", "Tag")
                        .WithMany("Branches")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Category", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Category", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.FifoExistence", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Movement", "Movement")
                        .WithMany()
                        .HasForeignKey("MovementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Image", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.LifoExistence", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Movement", "Movement")
                        .WithMany()
                        .HasForeignKey("MovementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Movement", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Associate", "Associate")
                        .WithMany("Movements")
                        .HasForeignKey("AssociateId");

                    b.HasOne("Cardyan.Inventory.Models.Movement", "LinkedMovement")
                        .WithMany()
                        .HasForeignKey("LinkedMovementId");

                    b.HasOne("Cardyan.Inventory.Models.Warehouse", "LinkedWarehouse")
                        .WithMany()
                        .HasForeignKey("LinkedWarehouseId");

                    b.HasOne("Cardyan.Inventory.Models.MovementType", "Type")
                        .WithMany("Movements")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.MovementProduct", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Movement", "Movement")
                        .WithMany("Products")
                        .HasForeignKey("MovementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.MovementTag", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Movement", "Movement")
                        .WithMany("Tags")
                        .HasForeignKey("MovementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Tag", "Tag")
                        .WithMany("Movements")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Product", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.HasOne("Cardyan.Inventory.Models.MeasurementUnit", "MeasurementUnit")
                        .WithMany()
                        .HasForeignKey("MeasurementUnitId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.ProductStatistic", "Statistic")
                        .WithMany()
                        .HasForeignKey("StatisticId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.ProductProperty", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Product", "Product")
                        .WithMany("Properties")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Property", "Property")
                        .WithMany("Products")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.ProductTag", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Product", "Product")
                        .WithMany("Tags")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Tag", "Tag")
                        .WithMany("Products")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Property", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.PropertyType", "Type")
                        .WithMany("Properties")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.Warehouse", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Branch", "Branch")
                        .WithMany("Warehouses")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.WarehouseStatistic", "Statistic")
                        .WithMany()
                        .HasForeignKey("StatisticId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Valuation", "Valuation")
                        .WithMany("Warehouses")
                        .HasForeignKey("ValuationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseLocation", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("Locations")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseLocationTag", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Tag", "Tag")
                        .WithMany("Locations")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.WarehouseLocation", "Location")
                        .WithMany("Tags")
                        .HasForeignKey("WarehouseId", "LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseProduct", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Category", "Category")
                        .WithMany("WarehouseProducts")
                        .HasForeignKey("CategoryId");

                    b.HasOne("Cardyan.Inventory.Models.Product", "Product")
                        .WithMany("Warehouses")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.WarehouseProductStatistic", "Statistic")
                        .WithMany()
                        .HasForeignKey("StatisticId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("Products")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.WarehouseLocation", "Location")
                        .WithMany("Products")
                        .HasForeignKey("WarehouseId", "LocationId");
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseProductTag", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Tag", "Tag")
                        .WithMany("WarehouseProducts")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Inventory.Models.WarehouseTag", b =>
                {
                    b.HasOne("Cardyan.Inventory.Models.Tag", "Tag")
                        .WithMany("Warehouses")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("Tags")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
