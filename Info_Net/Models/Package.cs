using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace Info_Net.Models
{
	public class Package
	{
		[Key]
		public int Package_id { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		[Display(Name ="Nombre del paquete")]
		public string NombrePack { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		[Display(Name ="Ancho de banda")]
		public int Banda { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		[RegularExpression("^\\d+$", ErrorMessage = "El stock debe contener sólo números.")]
		public string Precio { get; set; }
		public virtual ICollection <Bill> Bills { get; set; }
	}
}