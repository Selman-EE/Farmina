using Farmina.Web.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL
{
	public class FarminaContext : DbContext
	{
		public FarminaContext() : base("FarminaDbContext")
		{ }

		public DbSet<Company> Companies { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//for entity properties
			modelBuilder.Entity<Product>().Property(e => e.Price).HasPrecision(18, 2);
		}
	}
}