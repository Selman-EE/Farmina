using Farmina.Web.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL
{
	public class FarminaContext : DbContext
	{
		public FarminaContext() : base("FarminaDbContext")
		{ }

		public DbSet<Company> Companies { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<Discount> Discounts { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
			//for entity properties
			modelBuilder.Entity<OrderProduct>().Property(e => e.Price).HasPrecision(18, 4);
		}
	}
}