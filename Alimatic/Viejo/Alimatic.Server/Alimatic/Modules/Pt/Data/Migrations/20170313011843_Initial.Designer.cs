using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Alimatic.Pt.Data;
using Alimatic.Pt.Models;

namespace Alimatic.Server.Alimatic.Pt.Data.Migrations
{
    [DbContext(typeof(PtDbContext))]
    [Migration("20170313011843_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Alimatic.Pt.Models.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("Enabled");

                    b.Property<DateTime?>("EndingDate");

                    b.Property<DateTime>("ListedDate");

                    b.Property<int>("RepetitionId");

                    b.HasKey("Id");

                    b.HasIndex("RepetitionId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.ActivityCriterion", b =>
                {
                    b.Property<int>("ActivityId");

                    b.Property<int>("CriterionId");

                    b.Property<int>("SubCriterionId");

                    b.HasKey("ActivityId", "CriterionId", "SubCriterionId");

                    b.HasIndex("CriterionId", "SubCriterionId");

                    b.ToTable("ActivityCriterion");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.ActivityDocument", b =>
                {
                    b.Property<int>("ActivityId");

                    b.Property<int>("DocumentId");

                    b.HasKey("ActivityId", "DocumentId");

                    b.HasIndex("DocumentId");

                    b.ToTable("ActivityDocument");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.Charge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(127);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Charges");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.Completion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.HasKey("Id");

                    b.ToTable("Completions");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.Criterion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(127);

                    b.HasKey("Id");

                    b.ToTable("Criteria");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.Repetition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Value");

                    b.HasKey("Id");

                    b.ToTable("Repetitions");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.SubCriterion", b =>
                {
                    b.Property<int>("CriterionId");

                    b.Property<int>("SubCriterionId");

                    b.Property<string>("Name")
                        .HasMaxLength(127);

                    b.HasKey("CriterionId", "SubCriterionId");

                    b.ToTable("SubCriteria");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChargeId");

                    b.HasKey("Id");

                    b.HasIndex("ChargeId")
                        .IsUnique();

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.WorkerActivity", b =>
                {
                    b.Property<int>("ActivityId");

                    b.Property<int>("WorkerId");

                    b.Property<string>("Observations");

                    b.HasKey("ActivityId", "WorkerId");

                    b.HasIndex("WorkerId");

                    b.ToTable("WorkerActivities");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.WorkerActivityCompletion", b =>
                {
                    b.Property<int>("WorkerActivityWorkerId");

                    b.Property<int>("WorkerActivityActivityId");

                    b.Property<int>("CompletionId");

                    b.Property<string>("Observations");

                    b.HasKey("WorkerActivityWorkerId", "WorkerActivityActivityId", "CompletionId");

                    b.HasIndex("CompletionId");

                    b.HasIndex("WorkerActivityActivityId", "WorkerActivityWorkerId");

                    b.ToTable("WorkerActivityCompletions");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.Activity", b =>
                {
                    b.HasOne("Alimatic.Pt.Models.Repetition", "Repetition")
                        .WithMany()
                        .HasForeignKey("RepetitionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Pt.Models.ActivityCriterion", b =>
                {
                    b.HasOne("Alimatic.Pt.Models.Activity", "Activity")
                        .WithMany("Criteria")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Pt.Models.Criterion", "Criterion")
                        .WithMany()
                        .HasForeignKey("CriterionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Pt.Models.SubCriterion", "SubCriterion")
                        .WithMany("Activities")
                        .HasForeignKey("CriterionId", "SubCriterionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Pt.Models.ActivityDocument", b =>
                {
                    b.HasOne("Alimatic.Pt.Models.Activity", "Activity")
                        .WithMany("Documents")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Pt.Models.Document", "Document")
                        .WithMany("Activities")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Pt.Models.SubCriterion", b =>
                {
                    b.HasOne("Alimatic.Pt.Models.Criterion", "Criterion")
                        .WithMany("SubCriteria")
                        .HasForeignKey("CriterionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Pt.Models.Worker", b =>
                {
                    b.HasOne("Alimatic.Pt.Models.Charge", "Charge")
                        .WithOne("Worker")
                        .HasForeignKey("Alimatic.Pt.Models.Worker", "ChargeId");
                });

            modelBuilder.Entity("Alimatic.Pt.Models.WorkerActivity", b =>
                {
                    b.HasOne("Alimatic.Pt.Models.Activity", "Activity")
                        .WithMany("Workers")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Pt.Models.Worker", "Worker")
                        .WithMany("Activities")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Alimatic.Pt.Models.WorkerActivityCompletion", b =>
                {
                    b.HasOne("Alimatic.Pt.Models.Completion", "Completion")
                        .WithMany("WorkerActivities")
                        .HasForeignKey("CompletionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Alimatic.Pt.Models.WorkerActivity", "WorkerActivity")
                        .WithMany("Completions")
                        .HasForeignKey("WorkerActivityActivityId", "WorkerActivityWorkerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
