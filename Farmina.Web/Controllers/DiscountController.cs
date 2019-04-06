using Farmina.Web.BLL.Dto;
using Farmina.Web.DAL.Entity;
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
			return View(_fR.GetByDescending<Discount>(o => o.Id));
		}
		public ActionResult Create()
		{
			return View(new Discount());
		}
		[HttpPost]
		public ActionResult Create(Discount discount)
		{
			_fR.Add(discount);
			_fR.SaveChanges();

			return RedirectToAction("Index");
		}
		public ActionResult Edit(int id)
		{
			var discount = _fR.GetWhere<Discount>(s => s.Id == id)?.FirstOrDefault() ?? new Discount();
			if (discount.Id <= 0)
				return RedirectToAction("Index");

			return View("Create", discount);
		}
		[HttpPost]
		public ActionResult Edit(Discount discount)
		{
			var discountEntity = _fR.GetWhere<Discount>(s => s.Id == discount.Id)?.FirstOrDefault() ?? new Discount();
			if (discount.Id <= 0)
				return RedirectToAction("Index");
			//
			discountEntity.Name = discount.Name?.Trim();
			discountEntity.FirstPercent = discount.FirstPercent;
			discountEntity.SecondPercent = discount.SecondPercent;
			discountEntity.ThirdPercent = discount.ThirdPercent;
			discountEntity.FourthPercent = discount.FourthPercent;
			//
			_fR.Update(discountEntity);
			_fR.SaveChanges();
			//
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var discountEntity = _fR.GetWhere<Discount>(s => s.Id == id)?.FirstOrDefault() ?? new Discount();
			//
			_fR.Remove(discountEntity);
			_fR.SaveChanges();
			return Json(new Response { Status = true, EntityId = id, Message = "Silindi", TotalCount = 1 });
		}
	}
}