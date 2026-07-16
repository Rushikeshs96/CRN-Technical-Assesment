using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Item>().ToTable("Item");

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
