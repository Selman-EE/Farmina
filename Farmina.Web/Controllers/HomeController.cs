using Farmina.Web.DAL.Entity;
using Farmina.Web.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Farmina.Web.Controllers
{
	public class HomeController : Controller
	{

		public HomeController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}

		private readonly IFarminaRepository _fR;

		// GET: Home
		public ActionResult Index()
		{


			ViewBag.CompanyList = _fR.Get<Supplier>(o => o.Name).First();

			return View();
		}
	}
}