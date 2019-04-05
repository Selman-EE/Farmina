using Farmina.Web.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Farmina.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IFarminaRepository _fR;
		public ProductController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}


		// GET: Product
		public ActionResult Index()
		{
			return View();
		}
	}
}