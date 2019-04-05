using Farmina.Web.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Farmina.Web.Controllers
{
	public class DiscountController : Controller
	{
		private readonly IFarminaRepository _fR;
		public DiscountController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}


		// GET: Discount
		public ActionResult Index()
		{
			return View();
		}
	}
}