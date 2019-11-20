/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#if !POMELO
using MySQL.Data.EntityFrameworkCore.Extensions;
#endif

namespace Alimatic.Pt.Data
{
    using Models;

    public class PtDbContext : DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Criterion> Criteria { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Repetition> Repetitions { get; set; }
        public DbSet<Completion> Completions { get; set; }
        public DbSet<SubCriterion> SubCriteria { get; set; }
        public DbSet<WorkerActivity> WorkerActivities { get; set; }
        public DbSet<ActivityCriterion> ActivityCriteria { get; set; }
        public DbSet<ActivityCriterion> ActivityDocuments { get; set; }
        public DbSet<WorkerActivityCompletion> WorkerActivityCompletions { get; set; }

        public PtDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Pt)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Charge>().HasIndex(p => p.Name).IsUnique();

            modelBuilder.Entity<WorkerActivity>().HasKey(p => new { p.ActivityId, p.WorkerId });

            modelBuilder.Entity<ActivityDocument>().HasKey(p => new { p.ActivityId, p.DocumentId });

            modelBuilder.Entity<SubCriterion>().HasKey(p => new { p.CriterionId, p.SubCriterionId });

            modelBuilder.Entity<ActivityCriterion>().HasKey(p => new { p.ActivityId, p.CriterionId, p.SubCriterionId });

            modelBuilder.Entity<WorkerActivityCompletion>().HasKey(p => new { p.WorkerActivityWorkerId, p.WorkerActivityActivityId, p.CompletionId });

            modelBuilder.Entity<Charge>().HasOne(p => p.Worker).WithOne(p => p.Charge).HasForeignKey<Worker>(p => p.ChargeId).OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
/* { Alimatic.Server } */
