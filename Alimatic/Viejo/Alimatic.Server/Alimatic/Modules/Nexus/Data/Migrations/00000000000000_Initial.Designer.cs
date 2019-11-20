using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Alimatic.Nexus.Data;

namespace Alimatic.Nexus.Data.Migrations
{
    [DbContext(typeof(NexusDbContext))]
    [Migration("00000000000000_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Alimatic.Nexus.Models.Column", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EnumValues")
                        .HasMaxLength(16384);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<bool>("NotNull");

                    b.Property<int>("Order");

                    b.Property<int>("TableId");

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("TableId");

                    b.HasIndex("TypeId");

                    b.HasIndex("Name", "TableId")
                        .IsUnique();

                    b.ToTable("Columns");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.ColumnRole", b =>
                {
                    b.Property<int>("ColumnId");

                    b.Property<int>("RoleId");

                    b.Property<int>("PermissionId");

                    b.HasKey("ColumnId", "RoleId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("ColumnRoles");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.ColumnType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ColumnTypes");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.Permission", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.Row", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TableId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TableId");

                    b.HasIndex("UserId");

                    b.ToTable("Rows");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.RowColumn", b =>
                {
                    b.Property<int>("RowId");

                    b.Property<int>("ColumnId");

                    b.Property<string>("Value");

                    b.HasKey("RowId", "ColumnId");

                    b.HasAlternateKey("ColumnId", "RowId");

                    b.ToTable("RowColumns");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.Security", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Securities");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.Table", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.TableRole", b =>
                {
                    b.Property<int>("TableId");

                    b.Property<int>("RoleId");

                    b.Property<bool>("OverrideColumnsPermission");

                    b.Property<int>("PermissionId");

                    b.Property<int>("SecurityId");

                    b.HasKey("TableId", "RoleId");

                    b.HasAlternateKey("RoleId", "TableId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("SecurityId");

                    b.ToTable("TableRoles");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccountId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int>("SecurityId");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("SecurityId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAlternateKey("RoleId", "UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.Column", b =>
                {
                    b.HasOne("Alimatic.Nexus.Models.Table", "Table")
                        .WithMany("Columns")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Nexus.Models.ColumnType", "Type")
                        .WithMany("Columns")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.ColumnRole", b =>
                {
                    b.HasOne("Alimatic.Nexus.Models.Column", "Column")
                        .WithMany("Roles")
                        .HasForeignKey("ColumnId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Nexus.Models.Permission", "Permission")
                        .WithMany("ColumnSecurityList")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Nexus.Models.Role", "Role")
                        .WithMany("Columns")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.Row", b =>
                {
                    b.HasOne("Alimatic.Nexus.Models.Table", "Table")
                        .WithMany("Rows")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Nexus.Models.User", "User")
                        .WithMany("Rows")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.RowColumn", b =>
                {
                    b.HasOne("Alimatic.Nexus.Models.Column", "Column")
                        .WithMany("Rows")
                        .HasForeignKey("ColumnId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Nexus.Models.Row", "Row")
                        .WithMany("Columns")
                        .HasForeignKey("RowId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.TableRole", b =>
                {
                    b.HasOne("Alimatic.Nexus.Models.Permission", "Permission")
                        .WithMany("TableSecurityList")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Nexus.Models.Role", "Role")
                        .WithMany("Tables")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Nexus.Models.Security", "Security")
                        .WithMany("TableRoles")
                        .HasForeignKey("SecurityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Nexus.Models.Table", "Table")
                        .WithMany("Roles")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.User", b =>
                {
                    b.HasOne("Alimatic.Nexus.Models.Security", "Security")
                        .WithMany("Accounts")
                        .HasForeignKey("SecurityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Nexus.Models.UserRole", b =>
                {
                    b.HasOne("Alimatic.Nexus.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Nexus.Models.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
