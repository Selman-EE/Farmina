using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	[Table("Supplier")]
	public class Supplier : BaseEntity
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Tedarikçi Adı")]
		public string Name { get; set; }
		[DisplayName("Tedarikçi Kodu")]
		public string Code { get; set; }
	}
}