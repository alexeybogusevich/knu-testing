﻿using Core.Data.Configuration;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Order>? Order { get; set; }
        public DbSet<Customer>? Customers { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerEnityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        }
    }
}