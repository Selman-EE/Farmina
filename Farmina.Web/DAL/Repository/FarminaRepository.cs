using Farmina.Web.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Repository
{
	public class FarminaRepository : DbFactory, IFarminaRepository
	{
		private bool _disposed;

		//============================================================
		// Constructors
		//============================================================
		public FarminaRepository()
		{ }
		//============================================================
		// Business Logic
		//============================================================

		#region Generic methods
		//============================================================
		// List Finder
		//============================================================
		public List<T> Get<T>(Func<T, object> o) where T : class
		{
			return DbInstance.Set<T>().OrderBy(o).ToList();
		}

		public List<T> GetByDescending<T>(Func<T, object> o) where T : class
		{
			return DbInstance.Set<T>().OrderByDescending(o).ToList();
		}
		public List<T> GetByCertainCount<T>(Func<T, object> o, int takeCount) where T : class
		{
			return DbInstance.Set<T>().Take(takeCount).OrderByDescending(o).ToList();
		}

		public List<T> GetFiltered<T>(Func<T, object> o, Func<T, bool> s) where T : class
		{
			return DbInstance.Set<T>().Where(s).OrderBy(o).ToList();
		}

		public List<T> GetWhere<T>(Func<T, bool> s) where T : class
		{
			return DbInstance.Set<T>().Where(s).ToList();
		}
		public bool GetAny<T>(Func<T, bool> s) where T : class
		{
			return DbInstance.Set<T>().Any(s);
		}
		//============================================================
		// Entity Finder
		//============================================================
		public T Find<T>(Func<T, bool> s) where T : class
		{
			return DbInstance.Set<T>().SingleOrDefault(s);
		}

		//============================================================
		// Update Entity
		//============================================================
		public void Update<T>(T u) where T : class
		{
			DbInstance.Entry(u).State = EntityState.Modified;
		}

		//============================================================
		// Add Entity
		//============================================================


		public void Add<T>(T a) where T : class
		{
			DbInstance.Set<T>().Add(a);
		}

		public void AddList<T>(IEnumerable<T> a) where T : class
		{
			DbInstance.Set<T>().AddRange(a);
		}
		//============================================================
		// Remove Entity
		//============================================================
		public void Remove<T>(T u) where T : class
		{
			DbInstance.Entry(u).State = EntityState.Deleted;
		}

		//============================================================
		// Override Logic
		//============================================================
		public void SaveChanges()
		{
			DbInstance.SaveChanges();
		}

		//===========================================================
		// Overridden Business Logic
		//===========================================================

		/*
         * Disposes the context
         */
		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					DbInstance.Dispose();
				}
			}

			_disposed = true;
		}

		/*
         * Disposes the context
         */
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Customize methods
		public int GetLastVoucherNumber()
		{
			return DbInstance.Orders.Where(x => x.Status && !x.IsDeleted).OrderByDescending(o => o.VoucherNumber)?.FirstOrDefault()?.VoucherNumber ?? 0;
		}

		public List<Product> SearchProducts(string term, int takeCount)
		{
			if (string.IsNullOrEmpty(term))
				return new List<Product>();
			//
			return DbInstance.Products?.Where(x => x.Status && !x.IsDeleted)?.Where(x => x.Name.ToLower().Contains(term.Trim().ToLower()) ||
				 x.Barcode.ToLower().Contains(term.Trim().ToLower()) ||
				 x.Code.ToLower().Contains(term.Trim().ToLower()))?.Take(takeCount)?.ToList() ?? new List<Product>();
		}
		public List<Company> SearchCustomers(string term, int takeCount)
		{
			if (string.IsNullOrEmpty(term))
				return new List<Company>();
			//
			return DbInstance.Companies?.Where(x => x.Status && !x.IsDeleted)?.Where(x => x.Name.ToLower().Contains(term.Trim().ToLower())
				|| x.CustomerCode.ToLower().Contains(term.Trim().ToLower()))?.Take(takeCount)?.ToList() ?? new List<Company>();
		}
		public List<Supplier> SearchSuppliers(string term, int takeCount)
		{
			if (string.IsNullOrEmpty(term))
				return new List<Supplier>();
			//
			return DbInstance.Suppliers?.Where(x => x.Status && !x.IsDeleted)?.Where(x => x.Name.ToLower().Contains(term.Trim().ToLower())
				|| x.Code.ToLower().Contains(term.Trim().ToLower()))?.Take(takeCount)?.ToList() ?? new List<Supplier>();
		}

		public void DeleteLoginLogMoreThanThreeMonths()
		{
			DbInstance.AccountLogs.SqlQuery("DELETE FROM [AccountLog] WHERE LoggedTime <= @date", new SqlParameter("@date", DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd")));
		}

		#endregion


	}
}