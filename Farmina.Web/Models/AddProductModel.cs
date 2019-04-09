using Farmina.Web.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmina.Web.Models
{
	public class AddProductModel
	{
		public AddProductModel()
		{
			Products = new List<Product>();
			Discounts = new List<Discount>();
		}
		public List<Product> Products { get; set; }
		public List<Discount> Discounts { get; set; }
		public int ListIndex { get; set; }
	}
}