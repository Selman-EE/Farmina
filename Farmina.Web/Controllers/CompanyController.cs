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
	public class CompanyController : BaseController
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
			company.Status = true;
			_fR.Add(company);
			_fR.SaveChanges();

			return RedirectToAction("Index");
		}
		public ActionResult Edit(int id)
		{
			var company = _fR.GetWhere<Company>(s => s.Id == id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Company();
			if (company.Id <= 0)
				return RedirectToAction("Index");

			return View("Create", company);
		}
		[HttpPost]
		public ActionResult Edit(Company company)
		{
			var companyEntity = _fR.GetWhere<Company>(s => s.Id == company.Id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Company();
			if (company.Id <= 0)
				return RedirectToAction("Index");
			//
			companyEntity.Name = company.Name?.Trim();
			companyEntity.Country = company.Country?.Trim();
			companyEntity.CountryCode = company.CountryCode?.Trim();
			companyEntity.Region = company.Region?.Trim();
			companyEntity.TaxNumber = company.TaxNumber?.Trim();
			companyEntity.ZipCode = company.ZipCode?.Trim();
			companyEntity.City = company.City?.Trim();
			companyEntity.Address = company.Address?.Trim();
			companyEntity.CustomerCode = company.CustomerCode?.Trim();
			companyEntity.UpdatedDate = DateTime.Now;
			//
			_fR.Update(companyEntity);
			_fR.SaveChanges();
			//
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var companyEntity = _fR.GetWhere<Company>(s => s.Id == id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Company();
			companyEntity.Status = false;
			companyEntity.IsDeleted = true;
			companyEntity.DeletedDate = DateTime.Now;
			//
			_fR.Update(companyEntity);
			_fR.SaveChanges();
			return Json(new Response { Status = true, EntityId = id, Message = "Silindi", TotalCount = 1 });
		}
	}
}