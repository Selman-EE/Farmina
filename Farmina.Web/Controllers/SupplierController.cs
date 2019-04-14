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
	public class SupplierController : BaseController
	{
		private readonly IFarminaRepository _fR;
		public SupplierController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}

		// GET: Supplier
		public ActionResult Index()
		{
			return View(_fR.GetWhere<Supplier>(s => s.Status && !s.IsDeleted).OrderByDescending(o => o.Id));
		}
		public ActionResult Create()
		{
			return View(new Supplier());
		}
		[HttpPost]
		public ActionResult Create(Supplier supplier)
		{
			supplier.CreatedDate = DateTime.Now;
			supplier.Status = true;
			_fR.Add(supplier);
			_fR.SaveChanges();

			return RedirectToAction("Index");
		}
		public ActionResult Edit(int id)
		{
			var supplier = _fR.GetWhere<Supplier>(s => s.Id == id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Supplier();
			if (supplier.Id <= 0)
				return RedirectToAction("Index");

			return View("Create", supplier);
		}
		[HttpPost]
		public ActionResult Edit(Supplier supplier)
		{
			var supplierEntity = _fR.GetWhere<Supplier>(s => s.Id == supplier.Id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Supplier();
			if (supplier.Id <= 0)
				return RedirectToAction("Index");
			//
			supplierEntity.Name = supplier.Name?.Trim();
			supplierEntity.Code = supplier.Code?.Trim();
			supplierEntity.UpdatedDate = DateTime.Now;
			//
			_fR.Update(supplierEntity);
			_fR.SaveChanges();
			//
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var supplierEntity = _fR.GetWhere<Supplier>(s => s.Id == id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Supplier();
			supplierEntity.Status = false;
			supplierEntity.IsDeleted = true;
			supplierEntity.DeletedDate = DateTime.Now;
			//
			_fR.Update(supplierEntity);
			_fR.SaveChanges();
			return Json(new Response { Status = true, EntityId = id, Message = "Silindi", TotalCount = 1 });
		}
	}
}