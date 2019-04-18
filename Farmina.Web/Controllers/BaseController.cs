using Farmina.Web.DAL.Entity;
using Farmina.Web.DAL.Repository;
using Farmina.Web.Extension;
using Farmina.Web.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Farmina.Web.Controllers
{
	[CustomExceptionHandler]
	[AccountAuthorizeAttribute]
	public class BaseController : Controller
	{ }

	//========================================================================================================================
	// Attribute: Login Control and Role Control
	//========================================================================================================================
	#region Attribute: Login Control and Role Control
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class AccountAuthorizeAttribute : AuthorizeAttribute
	{
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			var user = CookieHelper.GetCookiesValue();
			if (user == null)
			{
				if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
				{
					filterContext.Result = new JsonResult() { Data = "unauthorized user", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
					return;
				}
				else
				{
					filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Account" }, { "Action", "Login" } });
					return;
				}
			}
			else
			{
				if (user.Username?.Trim().ToLower() != "Username".GetAppSetting().ToLower() || user.Password?.Trim() != "Password".GetAppSetting())
				{
					CookieHelper.ClearCookies();
					//
					if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
					{
						filterContext.Result = new JsonResult() { Data = "unauthorized user", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
						return;
					}
					else
					{
						filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Account" }, { "Action", "Login" } });
						return;
					}
				}
			}
		}
	}

	#endregion

	//========================================================================================================================
	// Custom Exception handler
	//========================================================================================================================
	#region Customize Exception Handler
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class CustomExceptionHandler : HandleErrorAttribute
	{
		private readonly IFarminaRepository _fR;
		public CustomExceptionHandler()
		{
			_fR = new FarminaRepository();
		}

		public override void OnException(ExceptionContext filterContext)
		{
			if (filterContext.ExceptionHandled || filterContext.HttpContext.IsCustomErrorEnabled)
			{
				return;
			}
			//
			//add mvc error to DB
			_fR.Add(new ErrorLog
			{
				Action = filterContext.RouteData.Values["action"].ToString(),
				Controller = filterContext.RouteData.Values["controller"].ToString(),
				ExceptionMessage = filterContext?.Exception?.Message ?? "",
				StackTrace = filterContext.Exception?.StackTrace ?? "",
				Date = filterContext.RequestContext.HttpContext.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
				RequestUrl = filterContext.HttpContext.Request.Url.ToString(),
				Browser = $"{filterContext.HttpContext.Request.Browser.Browser.ToString()} - Version:{filterContext.HttpContext.Request.Browser.Version.ToString()}"
			});

			filterContext.ExceptionHandled = true;
			filterContext.Result = new ViewResult()
			{
				ViewName = "Error"
			};
		}
	}

	#endregion


	public class DeleteFile : ActionFilterAttribute
	{
		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			filterContext.HttpContext.Response.Flush();
			var filePath = filterContext.HttpContext.Request.RawUrl.Split('?')[1];
			filePath = filePath.Substring(filePath.IndexOf('=') + 1);
			File.Delete(filePath);

		}

	}

}