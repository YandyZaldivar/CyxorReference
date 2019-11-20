using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Halo.Accounts.Data.Migrations
{
    [DbContext(typeof(AccountsDbContext))]
    partial class AccountsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Cyxor.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int>("Security");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Cyxor.Models.AccountRole", b =>
                {
                    b.Property<int>("AccountId");

                    b.Property<int>("RoleId");

                    b.HasKey("AccountId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AccountRoles");
                });

            modelBuilder.Entity("Cyxor.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(1024);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Cyxor.Models.AccountRole", b =>
                {
                    b.HasOne("Cyxor.Models.Account", "Account")
                        .WithMany("Roles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cyxor.Models.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
