namespace Farmina.Web.Migrations
{
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
			context.Companies.AddOrUpdate(new DAL.Entity.Company { Id = 1, Name = "Farmina", Code = "100" });
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.
		}
	}
}
