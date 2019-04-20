namespace Farmina.Web.Migrations
{
	using Farmina.Web.DAL.Entity;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<Farmina.Web.DAL.FarminaContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
		}

		protected override void Seed(Farmina.Web.DAL.FarminaContext context)
		{
			context.Suppliers.AddOrUpdate(new Supplier { Id = 1, Name = "test", Code = "100", Status = false, IsDeleted = true });
		}
	}
}
