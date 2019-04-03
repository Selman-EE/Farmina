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
		[Key, Column(Order = 0)]
		public int Id { get; set; }
		[Column(Order = 2)]
		public int SupplierId { get; set; }
		public int VoucherNumber { get; set; }
		public DateTime VoucherDate { get; set; }
		[ForeignKey("Company"), Column(Order = 1)]
		public int CompanyId { get; set; }

		/*
         * single relation
         */

		public virtual Company Company { get; set; }
		/*
         * multiple relation
         */

		public virtual ICollection<OrderProduct> OrderProducts { get; set; }
	}
}