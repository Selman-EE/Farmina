using Farmina.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Farmina.Web.Helper
{
	public class CookieHelper
	{
		private const string CookieName = "_FARPF";

		//clear cookie
		public static void ClearCookies()
		{
			if (HttpContext.Current.Request.Cookies[CookieName] != null)
				HttpContext.Current.Response.Cookies[CookieName].Expires = DateTime.Now.AddDays(-1);
		}
		public static LoginModel GetCookiesValue()
		{
			return GetLoginCookie();
		}
		public static void SetCookiesValue(LoginModel model)
		{
			SetLoginCookie(model);
		}


		//get value of the login info
		private static LoginModel GetLoginCookie()
		{
			if (HttpContext.Current.Request.Cookies.Get(CookieName) == null)
				return null;

			try
			{
				var data = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[CookieName].Value);
				return JsonConvert.DeserializeObject<LoginModel>(data.UserData);

			}
			catch (System.Security.Cryptography.CryptographicException ex)
			{
				FormsAuthentication.SignOut();
				HttpContext.Current.Request.Cookies.Set(new HttpCookie(CookieName, ""));
				return null;
			}
		}

		//create cookie for login info
		private static void SetLoginCookie(LoginModel model)
		{
			var userDataJson = JsonConvert.SerializeObject(model);

			//forms auth model
			var authTicket = new FormsAuthenticationTicket(3, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddMonths(1), true, userDataJson, "/");

			//encrypt the ticket and add it to a cookie
			HttpCookie cookie = new HttpCookie(CookieName, FormsAuthentication.Encrypt(authTicket));

			//set response cookie
			HttpContext.Current.Response.SetCookie(cookie);
		}
	}
}