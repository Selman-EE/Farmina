using Farmina.Web.DAL.Entity;
using Farmina.Web.DAL.Repository;
using Farmina.Web.Extension;
using Farmina.Web.Helper;
using Farmina.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Farmina.Web.Controllers
{
	public class AccountController : BaseController
	{
		private readonly IFarminaRepository _fR;
		public AccountController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}

		//================================
		// Business Logic
		//================================

		/*
        *  Login 
        */
		[AllowAnonymous]
		public ActionResult Login()
		{
			return View();
		}

		/*
        *  Login post 
        */
		[HttpPost]
		[AllowAnonymous]
		public ActionResult Login(LoginModel model)
		{
			//sing out all froms auth
			FormsAuthentication.SignOut();
			//
			//check out if values null
			if (model.Username.IsNull() || model.Password.IsNull())
			{
				//return error message
				ModelState.AddModelError("ErrorMessage", "Kullanıcı adı veya parola boş bırakılamaz Lütfen doldurunuz");
				return View("Login", model);
			}

			if (model.Username?.Trim().ToLower() != "Username".GetAppSetting().ToLower() || model.Password?.Trim() != "Admin".GetAppSetting())
			{
				//return error message
				ModelState.AddModelError("ErrorMessage", "Kullanıcı adı veya parola yanlış. Lütfen kontol edip tekrar deneyin");
				return View("Login", model);
			}
			//set session timeout
			System.Web.HttpContext.Current.Session.Timeout = 60;
			//	
			//save login data on cookie
			CookieHelper.SetCookiesValue(model);
			//
			//get ip address
#pragma warning disable CS0618 // Type or member is obsolete
			string hostName = System.Net.Dns.GetHostByName(hostName: Environment.MachineName).AddressList[0].ToString();
#pragma warning restore CS0618 // Type or member is obsolete

			_fR.Add(new AccountLog
			{
				HostName = hostName,
				UserHostAddress = Helper.RequestHelpers.GetClientIpAddress(Request),
				LogonUserIdentity = Request?.LogonUserIdentity?.Name ?? "",
				ConnectTime = DateTime.Now,
			});
			_fR.SaveChanges();
			//return main page
			return RedirectToAction("Index", "Home");
		}

		/*
        *  Login out
        */
		public ActionResult Logout()
		{
			//clear cookie
			CookieHelper.ClearCookies();
			//sing out all froms auth
			FormsAuthentication.SignOut();
			return View("Login");
		}
	}
}