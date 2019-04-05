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
	public class CompanyController : Controller
	{
		private readonly IFarminaRepository _fR;
		public CompanyController(IFarminaRepository farminaRepository)
		{
			_fR = farminaRepository;
		}


		// GET: Company
		public ActionResult Index()
		{
			return View(_fR.GetWhere<Company>(s => s.Status && !s.IsDeleted).OrderByDescending(o => o.Id));
		}
		public ActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Create(Company company)
		{
			company.CreatedDate = DateTime.Now;
			_fR.Add(company);
			return RedirectToAction("Index");
		}
		public ActionResult Edit(int companyId)
		{
			var company = _fR.GetWhere<Company>(s => s.Id == companyId)?.FirstOrDefault() ?? new Company();
			if (company.Id <= 0)
				return RedirectToAction("Index");

			return View(companyId);
		}
		[HttpPost]
		public ActionResult Edit(Company company)
		{
			var companyEntity = _fR.GetWhere<Company>(s => s.Id == company.Id)?.FirstOrDefault() ?? new Company();
			if (company.Id <= 0)
				return RedirectToAction("Index");
			//
			companyEntity.Name = company.Name?.Trim();
			companyEntity.Region = company.Region?.Trim();
			companyEntity.TaxNumber = company.TaxNumber?.Trim();
			companyEntity.ZipCode = company.ZipCode?.Trim();
			companyEntity.City = company.City?.Trim();
			companyEntity.Address = company.Address?.Trim();
			companyEntity.CustomerCode = company.CustomerCode?.Trim();
			companyEntity.Status = company.Status;
			companyEntity.UpdatedDate = DateTime.Now;
			//
			_fR.Update(companyEntity);
			//
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var companyEntity = _fR.GetWhere<Company>(s => s.Id == id)?.FirstOrDefault() ?? new Company();
			companyEntity.Status = false;
			companyEntity.IsDeleted = true;
			//
			_fR.Update(companyEntity);
			return Json(new Response { Status = true, EntityId = id, Message = "Silindi", TotalCount = 1 });
		}
	}
}