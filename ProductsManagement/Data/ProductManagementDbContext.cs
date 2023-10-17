using Microsoft.EntityFrameworkCore;
using ProductsManagement.Models.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProductsManagement.Data
{
    public class ProductManagementDbContext : DbContext
    {
        public ProductManagementDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Models.Entities.Attribute> Attributes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<AttributeValue>()
                .HasKey(av => new { av.ProductId, av.AttributeId });
        }
    }
    }
