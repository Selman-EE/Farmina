using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	public class OrderProduct
	{
		public string ProductName { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public string Discount { get; set; }
		public string DiscountName { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
		public decimal Total { get; set; }

		/*
	* foreign keys
	*/
		[Key, Column(Order = 0)]
		public int OrderId { get; set; }

		[Key, Column(Order = 1)]
		public int ProductId { get; set; }

		/*
         * single relation
         */

		public virtual Order Order { get; set; }

		public virtual Product Product { get; set; }


		/*
         * methods
         */

		public decimal CalculateTotalPrice()
		{
			var total = Price * Quantity;

			if (string.IsNullOrEmpty(Discount))
				return total;

			var discountRates = Discount.Split('+').Where(x => int.Parse(x) > 0).Select(s => int.Parse(s));
			foreach (var discount in discountRates)
			{
				total = total - (total * (discount / 100));
			}
			//
			return total;
		}

	}
}