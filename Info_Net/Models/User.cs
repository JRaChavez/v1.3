using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Info_Net.Models
{
	public class User
	{
		[Key]
		public int idUser { get; set; }

		[Display(Name ="Nombre de usuario")]
		[Required]
		[MaxLength(256, ErrorMessage ="El campo {0} debe tener un valor maximo de {1}")]
		public string Username { get; set; }


	}
}