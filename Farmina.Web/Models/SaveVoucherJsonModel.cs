using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmina.Web.Models
{
	[Serializable]
	public class SaveVoucherJsonModel
	{
		public SaveVoucherJsonModel()
		{
			Products = new List<OrderedProducts>();
		}

		public string PlatformCode { get; set; }
		public string VoucherDate { get; set; }
		public int VoucherNo { get; set; }
		public int CustomerId { get; set; }
		public int SupplierId { get; set; }
		public int TaxPercent { get; set; }
		public List<OrderedProducts> Products { get; set; }

	}

	[Serializable]
	public class OrderedProducts
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string ProductPrice { get; set; }
		public int ProductQuantity { get; set; }
		public string ProductDiscount { get; set; }
	}
}