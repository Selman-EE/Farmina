using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmina.Web.BLL.Dto
{
	public class ResponseExtended<T> : Response
	{
		public T Data { get; set; }
	}
}