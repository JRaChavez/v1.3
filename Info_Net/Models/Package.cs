using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Info_Net.Models
{
	public class Package
	{
		[Key]
		public int Package_id { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		public string NombrePack { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		public int Banda { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		public string Precio { get; set; }
		public virtual ICollection <Bill> Bills { get; set; }
	}
}