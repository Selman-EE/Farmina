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
	public class ProductController : BaseController
	{
		private readonly IFarminaRepository _fR;
		public ProductController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}


		// GET: Product
		public ActionResult Index()
		{
			return View(_fR.GetWhere<Product>(s => s.Status && !s.IsDeleted).OrderByDescending(o => o.Id));
		}
		public ActionResult Create()
		{
			return View(new Product());
		}
		[HttpPost]
		public ActionResult Create(Product product)
		{
			product.CreatedDate = DateTime.Now;
			product.Status = true;
			_fR.Add(product);
			_fR.SaveChanges();

			return RedirectToAction("Index");
		}
		public ActionResult Edit(int id)
		{
			var product = _fR.GetWhere<Product>(s => s.Id == id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Product();
			if (product.Id <= 0)
				return RedirectToAction("Index");

			return View("Create", product);
		}
		[HttpPost]
		public ActionResult Edit(Product product)
		{
			var productEntity = _fR.GetWhere<Product>(s => s.Id == product.Id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Product();
			if (product.Id <= 0)
				return RedirectToAction("Index");
			//
			productEntity.Name = product.Name?.Trim();
			productEntity.Code = product.Code?.Trim();
			productEntity.Barcode = product.Barcode?.Trim();
			productEntity.DefaultCount = product.DefaultCount;
			productEntity.Price = product.Price;
			productEntity.UpdatedDate = DateTime.Now;
			//
			_fR.Update(productEntity);
			_fR.SaveChanges();
			//
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var productEntity = _fR.GetWhere<Product>(s => s.Id == id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Product();
			productEntity.Status = false;
			productEntity.IsDeleted = true;
			productEntity.DeletedDate = DateTime.Now;
			//
			_fR.Update(productEntity);
			_fR.SaveChanges();
			return Json(new Response { Status = true, EntityId = id, Message = "Silindi", TotalCount = 1 });
		}
	}
}