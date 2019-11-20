﻿// <auto-generated />
using System;
using Cardyan.Accounting.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cardyan.Accounting.Data.Migrations
{
    [DbContext(typeof(CardyanDbContext))]
    [Migration("20180824191111_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Cardyan.Accounting.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Balance")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<int>("ClasificationId");

                    b.Property<string>("Code")
                        .HasMaxLength(126);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(126);

                    b.Property<int>("NormalBalanceId");

                    b.Property<int?>("ParentId");

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("ClasificationId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("NormalBalanceId");

                    b.HasIndex("ParentId");

                    b.HasIndex("TypeId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Cardyan.Accounting.Models.AccountClasification", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.HasKey("Id");

                    b.ToTable("AccountClasification");

                    b.HasData(
                        new { Id = 1, Name = "Real" },
                        new { Id = 2, Name = "Nominal" }
                    );
                });

            modelBuilder.Entity("Cardyan.Accounting.Models.AccountEntry", b =>
                {
                    b.Property<int>("AccountId");

                    b.Property<int>("TransactionId");

                    b.Property<decimal>("Amount")
                        .HasColumnType("Decimal(22, 2)");

                    b.Property<int>("TypeId");

                    b.HasKey("AccountId", "TransactionId");

                    b.HasIndex("TransactionId");

                    b.HasIndex("TypeId");

                    b.ToTable("AccountEntries");
                });

            modelBuilder.Entity("Cardyan.Accounting.Models.AccountEntryType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("AccountEntryTypes");

                    b.HasData(
                        new { Id = 1, Name = "Debit" },
                        new { Id = 2, Name = "Credit" }
                    );
                });

            modelBuilder.Entity("Cardyan.Accounting.Models.AccountNormalBalance", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("AccountNormalBalances");

                    b.HasData(
                        new { Id = 1, Name = "Debit" },
                        new { Id = 2, Name = "Credit" }
                    );
                });

            modelBuilder.Entity("Cardyan.Accounting.Models.AccountType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.HasKey("Id");

                    b.ToTable("AccountType");

                    b.HasData(
                        new { Id = 1, Name = "Active" },
                        new { Id = 2, Name = "Pasive" },
                        new { Id = 3, Name = "Equity" },
                        new { Id = 4, Name = "Revenus" },
                        new { Id = 5, Name = "Incons" }
                    );
                });

            modelBuilder.Entity("Cardyan.Accounting.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Cardyan.Accounting.Models.Account", b =>
                {
                    b.HasOne("Cardyan.Accounting.Models.AccountClasification", "Clasification")
                        .WithMany("Accounts")
                        .HasForeignKey("ClasificationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Accounting.Models.AccountNormalBalance", "NormalBalance")
                        .WithMany("Accounts")
                        .HasForeignKey("NormalBalanceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Accounting.Models.Account", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("Cardyan.Accounting.Models.AccountType", "Type")
                        .WithMany("Accounts")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cardyan.Accounting.Models.AccountEntry", b =>
                {
                    b.HasOne("Cardyan.Accounting.Models.Account", "Account")
                        .WithMany("Entries")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Accounting.Models.Transaction", "Transaction")
                        .WithMany("Entries")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cardyan.Accounting.Models.AccountEntryType", "Type")
                        .WithMany("Movements")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
