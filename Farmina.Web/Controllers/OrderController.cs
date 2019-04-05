using Farmina.Web.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Farmina.Web.Controllers
{
	public class OrderController : Controller
	{
		private readonly IFarminaRepository _fR;
		public OrderController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}


		// GET: Order
		public ActionResult Index()
		{
			return View();
		}
	}
}