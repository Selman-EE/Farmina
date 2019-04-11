using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Farmina.Web.DAL.Entity
{
	public class BaseEntity
	{
		[ScriptIgnore(ApplyToOverrides = true)]
		public bool Status { get; set; }
		[ScriptIgnore(ApplyToOverrides = true)]
		public bool IsDeleted { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		[ScriptIgnore(ApplyToOverrides = true)]
		public DateTime? DeletedDate { get; set; }
	}
}