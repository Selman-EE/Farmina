using Farmina.Web.DAL.Entity;
using Farmina.Web.DAL.Repository;
using Farmina.Web.Models;
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
		private readonly IFarminaRepository _fR;
		public HomeController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}

		// GET: Home
		public ActionResult Index()
		{
			return View();
		}

		[Route("home/addproducts")]
		public ActionResult AddProducts(List<int> productIds, int listIndex)
		{
			var products = _fR.GetWhere<Product>(x => productIds.Contains(x.Id) && x.Status && !x.IsDeleted);
			var discounts = _fR.GetByCertainCount<Discount>(o => o.Id, 10);
			//
			var data = new AddProductModel
			{
				Products = products,
				Discounts = discounts,
				ListIndex = listIndex
			};
			return PartialView(@"~\Views\Home\Partial\AddProduct.cshtml", data);
		}




		[HttpGet]
		[Route("home/searchproducts")]
		public JsonResult SearchProducts(string term)
		{
			var data = _fR.GetWhere<Product>(x => x.Name.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
				&& x.Barcode.ToLowerInvariant().Contains(term?.Trim() ?? "")
				&& x.Code.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
				&& x.Status && !x.IsDeleted)
				.Select(s => new { id = s.Id, text = s.Name });

			return Json(data, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		[Route("home/searchcustomers")]
		public JsonResult SearchCustomers(string term)
		{
			var data = _fR.GetWhere<Company>(x => x.Name.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
				&& x.CustomerCode.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
				&& x.Status && !x.IsDeleted)
				.Select(s => new { id = s.Id, text = s.Name });

			return Json(data, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		[Route("home/searchsuppliers")]
		public JsonResult SearchSuppliers(string term)
		{
			var data = _fR.GetWhere<Supplier>(x => x.Name.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
				&& x.Code.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
				&& x.Status && !x.IsDeleted)
				.Select(s => new { id = s.Id, text = s.Name });

			return Json(data, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		[Route("home/searchdiscount")]
		public JsonResult SearchDiscounts(string term)
		{
			var data = _fR.GetWhere<Discount>(x => x.Name.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
			&& x.ShowAllPercent.Contains(term?.Trim()?.ToLowerInvariant() ?? ""))
			.Select(s => new { id = s.ShowAllPercent, text = $"{s.Name} | {s.ShowAllPercent}", selected = false }).ToList();
			//
			data.Insert(0, new { id = "0", text = "indirim yok", selected = true });
			return Json(data, JsonRequestBehavior.AllowGet);

		}
	}
}