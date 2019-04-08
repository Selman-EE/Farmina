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
			return View(_fR.GetWhere<Order>(s => s.Status && !s.IsDeleted).OrderByDescending(o => o.Id));
		}
		public ActionResult Details(int id)
		{
			return View(_fR.GetWhere<Order>(x => x.Id == id).FirstOrDefault() ?? new Order());
		}
		[HttpPost]
		public ActionResult Delete(int id)
		{
			var orderEntity = _fR.GetWhere<Order>(s => s.Id == id && s.Status && !s.IsDeleted)?.FirstOrDefault() ?? new Order();
			orderEntity.Status = false;
			orderEntity.IsDeleted = true;
			orderEntity.DeletedDate = DateTime.Now;
			//
			_fR.Update(orderEntity);
			_fR.SaveChanges();
			return Json(new Response { Status = true, EntityId = id, Message = "Silindi", TotalCount = 1 });
		}

	}
}