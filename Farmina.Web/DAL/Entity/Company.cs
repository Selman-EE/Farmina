using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	[Table("Customer")]
	public class Company : BaseEntity
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Şirket Adı")]
		public string Name { get; set; }
		[DisplayName("Müşteri Kodu")]
		public string CustomerCode { get; set; }
		[DisplayName("Vergi No")]
		public string TaxNumber { get; set; }
		[DisplayName("Bölge")]
		public string Region { get; set; }
		[DisplayName("Posta Kodu")]
		public string ZipCode { get; set; }
		[DisplayName("Ülke")]
		public string Country { get; set; }
		[DisplayName("Şehir")]
		public string City { get; set; }
		[DisplayName("Adres")]
		public string Address { get; set; }
	}
}