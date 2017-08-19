using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Info_Net.Controllers
{
    public class EnviarCorreoController : Controller
    {
        // GET: SendEmail
        public ActionResult Index()
        {
            return View();
        }
		[HttpGet]
		public ActionResult EnviarCorreo()
		{
			return View();
		}
		[HttpPost]
		public ActionResult EnviarCorreo (string Para,string Asunto, string Mensaje, HttpPostedFileBase fichero)
		{
			try
			{
				MailMessage correo = new MailMessage();
				correo.From = new MailAddress("rafa696@live.com.mx");//correo de la empresa
				correo.To.Add(Para);
				correo.Subject = Asunto;
				correo.Body = Mensaje;
				correo.IsBodyHtml = true;
				correo.Priority = MailPriority.Normal;

				//configurar los servidores smtp
				//Hotmail
				SmtpClient client = new SmtpClient("smtp.live.com", 587);
                using (client)
                {
                    client.Credentials = new System.Net.NetworkCredential("rafa696@live.com.mx", "isabella");
                    client.EnableSsl = true;
                    client.Send(correo);
                }
				ViewBag.Mensaje = "Mensaje Enviado Correctamente";
			}
			catch (Exception ex)
			{

				ViewBag.Error = ex.Message;
			}
			return View();
		}
	}
}