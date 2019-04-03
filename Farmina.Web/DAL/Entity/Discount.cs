using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	public class Discount
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int FirstPercent { get; set; }
		public int SecondPercent { get; set; }
		public int ThirdPercent { get; set; }
		public int FourthPercent { get; set; }
	}
}