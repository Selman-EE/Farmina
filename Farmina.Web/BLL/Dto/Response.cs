using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmina.Web.BLL.Dto
{
	public class Response
	{
		public int EntityId { get; set; }
		public int TotalCount { get; set; }
		public string Message { get; set; }
		public bool Status { get; set; }
	}
}