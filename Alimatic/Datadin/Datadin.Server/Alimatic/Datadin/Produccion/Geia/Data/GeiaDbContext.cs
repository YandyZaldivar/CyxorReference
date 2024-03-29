﻿// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Datadin.Produccion.Geia.Data
{
    using Models;
    using Alimatic.Datadin.Produccion.Data;

    public class GeiaDbContext : DatadinDbContext
    {
        public GeiaDbContext() { }

        public GeiaDbContext(DbContextOptions<GeiaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => base.OnModelCreating(modelBuilder);

        public override void InitialSeeding(ModelBuilder modelBuilder)
        {
            var mapper = Network.Server.Mapper;

            modelBuilder.Entity<Division>().HasData(DivisionApiModel.GeiaDivisions.Select(p =>
                mapper.Map(p).ToANew<Division>(q => q.Map(s => s.Source.Id).To(t => t.Id))).ToArray());

            modelBuilder.Entity<Group>().HasData(GroupApiModel.GeiaGroups.Select(p =>
                mapper.Map(p).ToANew<Group>(q => q.Map(s => s.Source.Id).To(t => t.Id)
                    .And.Map(s => s.Source.DivisionId).To(t => t.DivisionId))).ToArray());

            modelBuilder.Entity<Enterprise>().HasData(EnterpriseApiModel.GeiaEnterprises.Select(p =>
                mapper.Map(p).ToANew<Enterprise>(q => q.Map(s => s.Source.Id).To(t => t.Id))).ToArray());

            modelBuilder.Entity<Frequency>().HasData(FrequencyApiModel.Frequencies.Select(p =>
                mapper.Map(p).ToANew<Frequency>(q => q.Map(s => s.Source.Id).To(t => t.Id))).ToArray());

            base.InitialSeeding(modelBuilder);
        }
    }
}
// { Alimatic.Datadin } - Backend
