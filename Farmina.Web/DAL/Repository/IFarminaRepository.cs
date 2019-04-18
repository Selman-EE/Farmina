using Farmina.Web.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Repository
{
	public interface IFarminaRepository
	{
		T Find<T>(Func<T, bool> s) where T : class;
		List<T> Get<T>(Func<T, object> o) where T : class;
		List<T> GetByDescending<T>(Func<T, object> o) where T : class;
		List<T> GetFiltered<T>(Func<T, object> o, Func<T, bool> s) where T : class;
		List<T> GetByCertainCount<T>(Func<T, object> o, int takeCount) where T : class;
		List<T> GetWhere<T>(Func<T, bool> s) where T : class;
		bool GetAny<T>(Func<T, bool> s) where T : class;
		void Add<T>(T a) where T : class;
		void AddList<T>(IEnumerable<T> a) where T : class;
		void Update<T>(T u) where T : class;
		void Remove<T>(T u) where T : class;
		void SaveChanges();
		void Dispose();

		//Custom methods
		int GetLastVoucherNumber();

		List<Product> SearchProducts(string term, int takeCount);
		List<Company> SearchCustomers(string term, int takeCount);
		List<Supplier> SearchSuppliers(string term, int takeCount);
		void DeleteLoginLogMoreThanThreeMonths();
	}
}