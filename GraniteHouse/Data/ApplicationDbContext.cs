using System;
using System.Collections.Generic;
using System.Text;
using GraniteHouse.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductTypes> ProductTypes { get; set; }
        public DbSet<SpecialTags> SpecialTags { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<ProductsSelectedForAppointment> ProductsSelectedForAppointment { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductsSelectedForAppointment>()
                .HasKey(c => new { c.AppointmentId, c.ProductId });

            modelBuilder.Entity<ProductsSelectedForAppointment>()
               .Property(c=>c.TotalQuantity).HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Products>()
              .Property(c => c.Price).HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Appointments>()
              .Property(c => c.TotalAppointment).HasColumnType("decimal(18,4)");

            modelBuilder.Entity<ProductsSelectedForAppointment>()
             .Property(c => c.price).HasColumnType("decimal(18,4)");
        }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
