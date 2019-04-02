using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	public class BaseEntity
	{
		public bool Status { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		public DateTime DeletedDate { get; set; }
	}
}