using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	[Table("Discount")]
	public class Discount
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Indirimin ismi")]
		public string Name { get; set; }
		[DisplayName("Birinci Indirim Oranı")]
		public int FirstPercent { get; set; }
		[DisplayName("İkinci Indirim Oranı")]
		public int SecondPercent { get; set; }
		[DisplayName("Üçüncü Indirim Oranı")]
		public int ThirdPercent { get; set; }
		[DisplayName("Dördüncü Indirim Oranı")]
		public int FourthPercent { get; set; }

		[NotMapped]
		public string ShowAllPercent => $"{FirstPercent}+{SecondPercent}+{ThirdPercent}+{FourthPercent}";
	}
}