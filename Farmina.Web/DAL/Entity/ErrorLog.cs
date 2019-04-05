using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	[Table("ErrorLog")]
	public class ErrorLog
	{
		[Key]
		public int Id { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public string Date { get; set; }
		public string RequestUrl { get; set; }
		public string ExceptionMessage { get; set; }
		public string CreatedDate { get; set; }
		public string StackTrace { get; set; }
		public string Browser { get; set; }
	}
}