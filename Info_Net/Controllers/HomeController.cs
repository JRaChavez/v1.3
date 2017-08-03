using Info_Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Info_Net.Controllers
{
	public class HomeController : Controller
	{
		private readonly MarkerRepository _markerRepository;
		public HomeController()
		{
			_markerRepository = new MarkerRepository();
		}
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Sync()
		{
			return View(_markerRepository.GetMarkers());
		}

		public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

		[HttpGet]
		public ActionResult Async()
		{
			return View();
		}

		[HttpPost]
		public ActionResult GetMarkersAsync()
		{
			return Json(_markerRepository.GetMarkers());
		}
	}
}