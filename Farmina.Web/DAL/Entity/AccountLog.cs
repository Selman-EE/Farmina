using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Farmina.Web.DAL.Entity
{
	[Table("AccountLog")]
	public class AccountLog
	{
		[Key]
		public int Id { get; set; }
		public string UserHostAddress { get; set; }
		public string HostName { get; set; }
		public string LogonUserIdentity { get; set; }
		public DateTime ConnectTime { get; set; }

	}
}