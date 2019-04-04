using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	[Table("Product")]
	public class Product : BaseEntity
	{
		public Product()
		{
			OrderProducts = new List<OrderProduct>();
		}


		[Key]
		public int Id { get; set; }
		[DisplayName("Ürün Adı")]
		public string Name { get; set; }
		[DisplayName("Ürün Kodu")]
		public string Code { get; set; }
		[DisplayName("Barkod")]
		public string Barcode { get; set; }
		[DisplayName("Fiyatı")]
		public decimal Price { get; set; }
		[DisplayName("Varsayılan Adet")]
		public int? DefaultCount { get; set; }

		/*
         * multiple relation
         */
		public virtual ICollection<OrderProduct> OrderProducts { get; set; }
	}
}