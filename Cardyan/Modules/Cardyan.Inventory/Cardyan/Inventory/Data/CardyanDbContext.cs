using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace Cardyan.Inventory.Data
{
    using Models;

    public class CardyanDbContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Valuation> Valuations { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }

        public DbSet<FifoExistence> FifoExistences { get; set; }
        public DbSet<LifoExistence> LifoExistences { get; set; }

        public DbSet<Associate> Associates { get; set; }
        public DbSet<AssociateTag> AssociateTags { get; set; }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchTag> BranchTags { get; set; }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<ProductStatistic> ProductStatistics { get; set; }

        public DbSet<Movement> Movements { get; set; }
        public DbSet<MovementTag> MovementTags { get; set; }
        public DbSet<MovementType> MovementTypes { get; set; }
        public DbSet<MovementProduct> MovementProducts { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseTag> WarehouseTags { get; set; }
        public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
        public DbSet<WarehouseLocation> WarehouseLocations { get; set; }
        public DbSet<WarehouseLocationTag> WarehouseLocationTags { get; set; }
        public DbSet<WarehouseStatistic> WarehouseStatistics { get; set; }
        public DbSet<WarehouseProductTag> WarehouseProductTags { get; set; }
        public DbSet<WarehouseProductStatistic> WarehouseProductStatistics { get; set; }

        public CardyanDbContext() { }

        public CardyanDbContext(DbContextOptions<CardyanDbContext> options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.EnableSensitiveDataLogging();
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Branch>().HasIndex(p => p.Code).IsUnique();
            modelBuilder.Entity<Branch>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.Code).IsUnique();
            //modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Property>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Warehouse>().HasIndex(p => p.Code).IsUnique();
            modelBuilder.Entity<Associate>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Valuation>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<PropertyType>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<MovementType>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<MeasurementUnit>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<WarehouseLocation>().HasIndex(p => p.Code).IsUnique();

            modelBuilder.Entity<Warehouse>().HasIndex(p => new { p.BranchId, p.Name }).IsUnique();
            modelBuilder.Entity<WarehouseLocation>().HasIndex(p => new { p.WarehouseId, p.Name }).IsUnique();

            modelBuilder.Entity<BranchTag>().HasKey(p => new { p.BranchId, p.TagId });
            modelBuilder.Entity<ProductTag>().HasKey(p => new { p.ProductId, p.TagId });
            modelBuilder.Entity<MovementTag>().HasKey(p => new { p.MovementId, p.TagId });
            modelBuilder.Entity<WarehouseTag>().HasKey(p => new { p.WarehouseId, p.TagId });
            modelBuilder.Entity<AssociateTag>().HasKey(p => new { p.AssociateId, p.TagId });
            modelBuilder.Entity<MovementProduct>().HasKey(p => new { p.MovementId, p.ProductId });
            modelBuilder.Entity<ProductProperty>().HasKey(p => new { p.ProductId, p.PropertyId });
            modelBuilder.Entity<WarehouseProduct>().HasKey(p => new { p.WarehouseId, p.ProductId });
            modelBuilder.Entity<WarehouseLocation>().HasKey(p => new { p.WarehouseId, p.LocationId });
            modelBuilder.Entity<WarehouseProductTag>().HasKey(p => new { p.WarehouseId, p.ProductId, p.TagId });
            modelBuilder.Entity<FifoExistence>().HasKey(p => new { p.MovementId, p.WarehouseId, p.ProductId });
            modelBuilder.Entity<LifoExistence>().HasKey(p => new { p.MovementId, p.WarehouseId, p.ProductId });
            modelBuilder.Entity<WarehouseLocationTag>().HasKey(p => new { p.WarehouseId, p.LocationId, p.TagId });

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                property.Relational().ColumnType = $"{nameof(Decimal)}({22}, {2})";

            InitialSeeding(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        void InitialSeeding(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Valuation>().HasData(Valuation.Items);
            modelBuilder.Entity<PropertyType>().HasData(PropertyType.Items);
            modelBuilder.Entity<MovementType>().HasData(MovementType.Items);
        }
    }
}
