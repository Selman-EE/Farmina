﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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
	}
}