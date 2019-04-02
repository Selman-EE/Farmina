using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	public class Voucher
	{
		public int Id { get; set; }
		public int VoucherNumber { get; set; }
		public string VoucherData { get; set; }
		public DateTime VoucherDate { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}