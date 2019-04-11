using Farmina.Web.BLL.Dto;
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
			ViewBag.VoucherNo = _fR.GetLastVoucherNumber();

			return View();
		}
		[Route("home/savevoucher")]
		public ActionResult Save(SaveVoucherJsonModel model)
		{
			var alreadyUsedVoucherNo = _fR.GetAny<Order>(x => x.VoucherNumber == model.VoucherNo);
			if (alreadyUsedVoucherNo)
				return Json(new Response { Status = false, Message = "Belge no daha önce kullanılmış" }, JsonRequestBehavior.AllowGet);

			using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
			{
				try
				{
					//
					//order date parse useable datetime format
					DateTime voucherDate;
					try
					{
						voucherDate = DateTime.ParseExact(model.VoucherDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
					}
					catch (Exception ex)
					{
						return Json(new Response { Status = false, Message = "Tarih formatı yanlış Lütfen sayfayı yenileyip tekrar deneyin" }, JsonRequestBehavior.AllowGet);
					}
					//
					var order = new Order
					{
						PlatformCode = model.PlatformCode,
						CompanyId = model.CustomerId,
						SupplierId = model.SupplierId,
						VoucherNumber = model.VoucherNo,
						VoucherDate = voucherDate,
						CreatedDate = DateTime.Now,
						Tax = model.TaxPercent,
						Status = true
					};
					//
					//save order
					_fR.Add(order);
					_fR.SaveChanges();

					var productList = new List<OrderProduct>();
					foreach (var item in model.Products)
					{
						var op = new OrderProduct
						{
							OrderId = order.Id,
							ProductId = item.ProductId,
							ProductName = item.ProductName,
							Discount = item.ProductDiscount,
							Quantity = item.ProductQuantity,
							Price = string.IsNullOrEmpty(item.ProductPrice) ? 0 : Convert.ToDecimal(item.ProductPrice),
						};
						//
						op.Total = op.CalculateTotalPrice();
						op.Total = op.Total + (op.Total * order.Tax) / 100;
						//
						productList.Add(op);
					}
					//
					//save order
					_fR.AddList(productList);
					_fR.SaveChanges();
					//
					scope.Complete();
					return Json(new Response { Status = true, EntityId = model.VoucherNo + 1, Message = "Belge kayıt edildi." }, JsonRequestBehavior.AllowGet);
				}
				catch (Exception ex)
				{
					scope.Dispose();
					return Json(new Response { Status = false, EntityId = 0, Message = ex.InnerException?.InnerException.Message ?? ex.Message, TotalCount = 0 });
				}
			}
		}

		[Route("home/addproducts")]
		public ActionResult AddProducts(string productIds, int listIndex)
		{
			var products = _fR.GetWhere<Product>(x => productIds.Split(',').Contains(x.Id.ToString()) && x.Status && !x.IsDeleted);
			var discounts = _fR.GetByDescending<Discount>(o => o.Id);
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
		[Route("home/getcustomer")]
		public JsonResult GetCustomerById(int id)
		{
			var data = _fR.Find<Company>(x => x.Id == id && x.Status && !x.IsDeleted);
			return Json(data, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		[Route("home/getsupplier")]
		public JsonResult GetSupplierById(int id)
		{
			var data = _fR.Find<Supplier>(x => x.Id == id && x.Status && !x.IsDeleted);
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[Route("home/searchproducts")]
		public JsonResult SearchProducts(string term)
		{
			var data = _fR.GetFiltered<Product>(o => o.Name,
				x => x.Name.ToLowerInvariant().Contains(term.Trim().ToLowerInvariant()) ||
				x.Barcode.ToLowerInvariant().Contains(term.Trim()) ||
				x.Code.ToLowerInvariant().Contains(term.Trim().ToLowerInvariant()))
				.Where(x => x.Status && !x.IsDeleted)
				.Select(s => new { id = s.Id, text = $"{s.Name} | {s.Barcode} | {s.Code}" });

			return Json(data, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		[Route("home/searchcustomers")]
		public JsonResult SearchCustomers(string term)
		{
			var data = _fR.GetWhere<Company>(x => x.Name.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
				|| x.CustomerCode.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? ""))
				.Where(x => x.Status && !x.IsDeleted)
				.Select(s => new { id = s.Id, text = $"{s.Name} ({s.CustomerCode})" });

			return Json(data, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		[Route("home/searchsuppliers")]
		public JsonResult SearchSuppliers(string term)
		{
			var data = _fR.GetWhere<Supplier>(x => x.Name.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
				|| x.Code.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? ""))
				.Where(x => x.Status && !x.IsDeleted)
				.Select(s => new { id = s.Id, text = $"{s.Name} ({s.Code})" });

			return Json(data, JsonRequestBehavior.AllowGet);
		}
		//[HttpGet]
		//[Route("home/searchdiscount")]
		//public JsonResult SearchDiscounts(string term)
		//{
		//	var data = _fR.GetWhere<Discount>(x => x.Name.ToLowerInvariant().Contains(term?.Trim()?.ToLowerInvariant() ?? "")
		//	&& x.ShowAllPercent.Contains(term?.Trim()?.ToLowerInvariant() ?? ""))
		//	.Select(s => new { id = s.ShowAllPercent, text = $"{s.Name} | {s.ShowAllPercent}", selected = false }).ToList();
		//	//
		//	data.Insert(0, new { id = "0", text = "indirim yok", selected = true });
		//	return Json(data, JsonRequestBehavior.AllowGet);

		//}
	}
}