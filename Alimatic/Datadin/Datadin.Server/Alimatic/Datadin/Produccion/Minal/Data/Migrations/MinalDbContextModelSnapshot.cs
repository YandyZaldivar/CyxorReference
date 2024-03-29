﻿// <auto-generated />
using System;
using Alimatic.Datadin.Produccion.Minal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Alimatic.Datadin.Produccion.Minal.Data.Migrations
{
    [DbContext(typeof(MinalDbContext))]
    partial class MinalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Division", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Divisions");

                    b.HasData(
                        new { Id = 1, Name = "Minal" }
                    );
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Enterprise", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("DivisionId");

                    b.Property<string>("FullName")
                        .HasMaxLength(127);

                    b.Property<int>("GroupId");

                    b.Property<string>("Name")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("FullName")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("DivisionId", "GroupId");

                    b.ToTable("Enterprises");

                    b.HasData(
                        new { Id = 100001, DivisionId = 1, FullName = "Empresa Mixta Cervecería Bucanero S.A.", GroupId = 1, Name = "Bucanero" },
                        new { Id = 100002, DivisionId = 1, FullName = "Bravo S.A.", GroupId = 1, Name = "Bravo" },
                        new { Id = 100003, DivisionId = 1, FullName = "Coracán S.A.", GroupId = 1, Name = "Coracán" },
                        new { Id = 100004, DivisionId = 1, FullName = "Coralac S.A.", GroupId = 1, Name = "Coralac" },
                        new { Id = 100005, DivisionId = 1, FullName = "Los Portales S.A.", GroupId = 1, Name = "Los Portales" },
                        new { Id = 100006, DivisionId = 1, FullName = "Industrial Molinera de la Habana S.A.", GroupId = 1, Name = "Molinera" },
                        new { Id = 100007, DivisionId = 1, FullName = "Stella S.A.", GroupId = 1, Name = "Stella" },
                        new { Id = 100008, DivisionId = 1, FullName = "Lefersa S.A.", GroupId = 1, Name = "Lefersa" },
                        new { Id = 100009, DivisionId = 1, FullName = "Nescor S.A.", GroupId = 1, Name = "Nescor" },
                        new { Id = 100010, DivisionId = 1, FullName = "Papas & Company S.A.", GroupId = 1, Name = "Papas & Company" },
                        new { Id = 200001, DivisionId = 1, FullName = "Las Lomas S.A.", GroupId = 2, Name = "Las Lomas" },
                        new { Id = 200002, DivisionId = 1, FullName = "Empresa de Alimentos y Bebidas la Estancia S.A.", GroupId = 2, Name = "La Estancia" },
                        new { Id = 200003, DivisionId = 1, FullName = "Havana Club International S.A.", GroupId = 2, Name = "Habana Club" },
                        new { Id = 300001, DivisionId = 1, FullName = "Empleadora S.A.", GroupId = 3, Name = "Empleadora" },
                        new { Id = 400001, DivisionId = 1, FullName = "Instituto de Investigaciones de la Industria Alimentaria", GroupId = 4, Name = "IIIA" },
                        new { Id = 400002, DivisionId = 1, FullName = "Oficina Nacional de Inspección Estatal", GroupId = 4, Name = "ONIE" },
                        new { Id = 400003, DivisionId = 1, FullName = "Centro de Investigaciones Pesqueras", GroupId = 4, Name = "CIP" },
                        new { Id = 400004, DivisionId = 1, FullName = "Instituto Marítimo Pesquero Andres Gonzalez Lines", GroupId = 4, Name = "Gonzalez Lines" },
                        new { Id = 400005, DivisionId = 1, FullName = "Unidad Presupuestada de Administración Interna", GroupId = 4, Name = "UPPD" },
                        new { Id = 400006, DivisionId = 1, FullName = "UP Organismo Central", GroupId = 4, Name = "UP Organismo Central" }
                    );
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Frequency", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.ToTable("Frequencies");

                    b.HasData(
                        new { Id = 1, Name = "Daily" },
                        new { Id = 2, Name = "Monthly" }
                    );
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Group", b =>
                {
                    b.Property<int>("DivisionId");

                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .HasMaxLength(16);

                    b.HasKey("DivisionId", "Id");

                    b.ToTable("Groups");

                    b.HasData(
                        new { DivisionId = 1, Id = 1, Name = "Coralsa" },
                        new { DivisionId = 1, Id = 2, Name = "Cubaron" },
                        new { DivisionId = 1, Id = 3, Name = "Empleadoras" },
                        new { DivisionId = 1, Id = 4, Name = "Presupuestadas" }
                    );
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Model", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("ColumnCount");

                    b.Property<string>("ColumnNames")
                        .HasMaxLength(1024);

                    b.Property<string>("Description")
                        .HasMaxLength(1024);

                    b.Property<int>("FrequencyId");

                    b.Property<bool>("IsEFModel");

                    b.Property<int>("RowCount");

                    b.HasKey("Id");

                    b.HasIndex("FrequencyId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Record", b =>
                {
                    b.Property<int>("Year");

                    b.Property<int>("Month");

                    b.Property<int>("Day");

                    b.Property<int>("EnterpriseId");

                    b.Property<int>("ModelId");

                    b.Property<int>("RowId");

                    b.Property<decimal?>("C01")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("C02")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("C03")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("C04")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("C05")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("C06")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("C07")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("C08")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<decimal?>("C09")
                        .HasColumnType("Decimal(22, 2)");

                    b.HasKey("Year", "Month", "Day", "EnterpriseId", "ModelId", "RowId");

                    b.HasAlternateKey("Day", "EnterpriseId", "ModelId", "Month", "RowId", "Year");

                    b.HasIndex("EnterpriseId");

                    b.HasIndex("ModelId");

                    b.HasIndex("RowId", "ModelId");

                    b.HasIndex("Year", "Month", "Day", "ModelId");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(1024);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Row", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("ModelId");

                    b.Property<string>("Description")
                        .HasMaxLength(1024);

                    b.HasKey("Id", "ModelId");

                    b.HasIndex("ModelId");

                    b.ToTable("Rows");
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Template", b =>
                {
                    b.Property<int>("Year");

                    b.Property<int>("Month");

                    b.Property<int>("Day");

                    b.Property<int>("ModelId");

                    b.Property<bool>("Locked");

                    b.HasKey("Year", "Month", "Day", "ModelId");

                    b.HasAlternateKey("Day", "ModelId", "Month", "Year");

                    b.HasIndex("ModelId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccountId");

                    b.Property<string>("Email")
                        .HasMaxLength(64);

                    b.Property<int?>("EnterpriseId");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<string>("Password")
                        .HasMaxLength(8192);

                    b.Property<int>("Permission");

                    b.Property<int>("SecurityLevel");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("EnterpriseId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.UserModel", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ModelId");

                    b.HasKey("UserId", "ModelId");

                    b.HasAlternateKey("ModelId", "UserId");

                    b.ToTable("UserModels");
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAlternateKey("RoleId", "UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Enterprise", b =>
                {
                    b.HasOne("Alimatic.Datadin.Produccion.Models.Division", "Division")
                        .WithMany("Enterprise")
                        .HasForeignKey("DivisionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Datadin.Produccion.Models.Group", "Group")
                        .WithMany("Enterprise")
                        .HasForeignKey("DivisionId", "GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Group", b =>
                {
                    b.HasOne("Alimatic.Datadin.Produccion.Models.Division", "Division")
                        .WithMany("Group")
                        .HasForeignKey("DivisionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Model", b =>
                {
                    b.HasOne("Alimatic.Datadin.Produccion.Models.Frequency", "Frequency")
                        .WithMany()
                        .HasForeignKey("FrequencyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Record", b =>
                {
                    b.HasOne("Alimatic.Datadin.Produccion.Models.Enterprise", "Enterprise")
                        .WithMany("EstadosFinancieros")
                        .HasForeignKey("EnterpriseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Datadin.Produccion.Models.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Datadin.Produccion.Models.Row", "Row")
                        .WithMany()
                        .HasForeignKey("RowId", "ModelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Datadin.Produccion.Models.Template", "Template")
                        .WithMany()
                        .HasForeignKey("Year", "Month", "Day", "ModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Row", b =>
                {
                    b.HasOne("Alimatic.Datadin.Produccion.Models.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.Template", b =>
                {
                    b.HasOne("Alimatic.Datadin.Produccion.Models.Model", "Model")
                        .WithMany("Templates")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.User", b =>
                {
                    b.HasOne("Alimatic.Datadin.Produccion.Models.Enterprise", "Enterprise")
                        .WithMany()
                        .HasForeignKey("EnterpriseId");
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.UserModel", b =>
                {
                    b.HasOne("Alimatic.Datadin.Produccion.Models.Model", "Model")
                        .WithMany("Users")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Datadin.Produccion.Models.User", "User")
                        .WithMany("Models")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Datadin.Produccion.Models.UserRole", b =>
                {
                    b.HasOne("Alimatic.Datadin.Produccion.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Datadin.Produccion.Models.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
