using Farmina.Web.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Farmina.Web.Controllers
{
	public class SupplierController : Controller
	{
		private readonly IFarminaRepository _fR;
		public SupplierController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}


		// GET: Supplier
		public ActionResult Index()
		{
			return View();
		}
	}
}