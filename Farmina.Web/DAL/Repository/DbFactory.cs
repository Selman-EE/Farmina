using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;

namespace Farmina.Web.DAL.Repository
{
	public class DbFactory
	{
		//connection string name in web config
		private const string DbContextKey = "FarminaDbContext";

		public static FarminaContext DbInstance
		{
			get
			{
				return InstanceContext();
			}
		}

		public static void RemoveContext()
		{
			if (HttpContext.Current != null && HttpContext.Current.Items[DbContextKey] != null)
			{
				((ObjectContext)HttpContext.Current.Items[DbContextKey]).Dispose();
				HttpContext.Current.Items.Remove(DbContextKey);
			}
		}

		public static FarminaContext InstanceContext()
		{

			if (!System.Web.Hosting.HostingEnvironment.IsHosted)  //Console and windows applications use (!!İmportant after cache develops)
			{
				return SingletonContext.Instance;
			}


			if (!HttpContext.Current.Items.Contains(DbContextKey))
			{
				HttpContext.Current.Items.Add(DbContextKey, new FarminaContext());
			}


			System.Data.Entity.Infrastructure.Interception.DbInterception.Add(new NoLockInterceptor());

			return HttpContext.Current.Items[DbContextKey] as FarminaContext;
		}
	}


	public sealed class SingletonContext
	{
		private static volatile FarminaContext _instance;
		public static object SyncRoot { get; } = new object();
		private SingletonContext() { }

		public static FarminaContext Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (SyncRoot)
					{
						if (_instance == null)
							_instance = new FarminaContext();
					}
				}

				return _instance;
			}
		}
	}


	#region NoLock interceptor

	public class NoLockInterceptor : System.Data.Entity.Infrastructure.Interception.DbCommandInterceptor
	{

		private static readonly Regex TableAliasRegex = new Regex(@"(?<tableAlias>AS \[Extent\d+\](?! WITH \(NOLOCK\)))", RegexOptions.Multiline | RegexOptions.IgnoreCase);

		/// <summary>
		/// Add "WITH (NOLOCK)" hint to SQL queries - unique to each thread 
		/// (set to true only when needed and then back to false)
		/// </summary>
		[ThreadStatic]
		public static bool AddNoLockHintToSqlQueries;

		public NoLockInterceptor()
		{
			AddNoLockHintToSqlQueries = false;
		}

		public override void ScalarExecuting(System.Data.Common.DbCommand command, System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext<object> interceptionContext)
		{
			if (AddNoLockHintToSqlQueries)
			{
				command.CommandText = TableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
			}
		}

		public override void ReaderExecuting(System.Data.Common.DbCommand command, System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
		{
			if (AddNoLockHintToSqlQueries)
			{
				command.CommandText = TableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
			}
		}
	}

	public static class IQueryableExtension
	{
		public static List<T> ToListIsolation<T>(this IQueryable<T> query, System.Transactions.IsolationLevel isolationLevel = System.Transactions.IsolationLevel.Unspecified)
		{
			using (var scope = new TransactionScope(TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = isolationLevel }))
			{
				if (isolationLevel == System.Transactions.IsolationLevel.ReadUncommitted)
					NoLockInterceptor.AddNoLockHintToSqlQueries = true;

				List<T> toReturn = query.ToList();
				scope.Complete();


				if (isolationLevel == System.Transactions.IsolationLevel.ReadUncommitted)
					NoLockInterceptor.AddNoLockHintToSqlQueries = false;

				return toReturn;
			}
		}
	}

	#endregion
}