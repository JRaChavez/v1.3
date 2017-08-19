using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Info_Net.Models
{
	public class Bill
	{
		[Key]
		public int Bill_id { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		[Display(Name ="Nombre del Solisitante")]
		public string Nombres { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		[Display(Name ="Apellido Paterno")]
		public string Apaterno{ get; set; }

		[Display(Name = "Apellido Paterno")]
		public string Amaterno { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		public int Telefono { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		[Display(Name ="Correo electronico")]
		public string Email { get; set; }

		//paquete
		[Required(ErrorMessage = "the file {0} is required")]
		[Display(Name ="Seleccione un paquete")]
		public int Package_id { get; set; }


		//DIreccion
		[Required(ErrorMessage = "the file {0} is required")]
		public string Calle { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		[Display(Name ="Numero de casa")]
		public int NumEx { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		public string Estado { get; set; }

		[Required(ErrorMessage = "the file {0} is required")]
		public string Municipio { get; set; }

		
		[Required(ErrorMessage = "the file {0} is required")]
		[Display(Name ="Colonia")]
		public string colonia { get; set; }

		public virtual  Package Package { get; set; }

	}
}