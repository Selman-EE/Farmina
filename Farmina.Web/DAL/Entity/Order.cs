using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	[Table("Order")]
	public class Order : BaseEntity
	{
		public Order()
		{
			OrderProducts = new List<OrderProduct>();
		}

		[Key, Column(Order = 0)]
		public int Id { get; set; }

		public int VoucherNumber { get; set; }
		public DateTime VoucherDate { get; set; }
		public string VoucherJsonData { get; set; }
		public int Tax { get; set; }
		[ForeignKey("Company"), Column(Order = 1)]
		public int CompanyId { get; set; }
		[ForeignKey("Supplier"), Column(Order = 2)]
		public int SupplierId { get; set; }

		/*
         * single relation
         */

		public virtual Company Company { get; set; }
		public virtual Supplier Supplier { get; set; }
		/*
         * multiple relation
         */

		public virtual ICollection<OrderProduct> OrderProducts { get; set; }
	}
}