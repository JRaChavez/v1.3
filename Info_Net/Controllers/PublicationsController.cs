using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Info_Net.Models;
using Info_Net.Clases;

namespace Info_Net.Controllers
{
    public class PublicationsController : Controller
    {
        private InfoNetContex db = new InfoNetContex();
		private FilesHelper view =new FilesHelper();

		// GET: Publications
		public ActionResult Index()
        {
            return View(db.Publications.ToList());
        }

        // GET: Publications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publications.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // GET: Publications/Create
		
        public ActionResult Create()
        {
            return View();
        }

		// POST: Publications/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Publication publication)
        {
			if (ModelState.IsValid)
			{

				db.Publications.Add(publication);
				var response = DBHelper.SaveChanges(db);

				if (response.Succeded)
				{
					if (publication.ImagenFile != null)
					{
						var folder = "~/Content/Fotos";
						var file = string.Format("{0}.jpg", publication.Publication_id);
						var responsePhoto = FilesHelper.UploadPhoto(publication.ImagenFile, folder, file);

						if (responsePhoto)
						{
							var pic = string.Format("{0}/{1}", folder, file);
							publication.Imagen = pic;
							db.Entry(publication).State = EntityState.Modified;
							var responsePhotoUpload = DBHelper.SaveChanges(db);

							if (responsePhotoUpload.Succeded)
							{
								return RedirectToAction("Index");
							}

							ModelState.AddModelError(string.Empty, responsePhotoUpload.Message);
						}
					}
					return RedirectToAction("Index");
				}
				ModelState.AddModelError(string.Empty, response.Message);
			}
			return View(publication);
		}

		// GET: Publications/Edit/5
		[Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publications.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // POST: Publications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Publication publication)
        {
			if (ModelState.IsValid)
			{
				if (publication.ImagenFile != null)
				{
					var pic = string.Empty;
					var folder = "~/Content/Fotos";
					var file = string.Format("{0}.jpg", publication.Publication_id);
					var responsePhoto = FilesHelper.UploadPhoto(publication.ImagenFile, folder, file);

					if (responsePhoto)
					{
						pic = string.Format("{0}/{1}", folder, file);
						publication.Imagen = pic;
					}
				}

				db.Entry(publication).State = EntityState.Modified;

				var responsePhotoUpload = DBHelper.SaveChanges(db);

				if (responsePhotoUpload.Succeded)
				{
					return RedirectToAction("Index");
				}
				ModelState.AddModelError(string.Empty, responsePhotoUpload.Message);
			}

			return View(publication);
        }
		[Authorize]
        // GET: Publications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publications.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Publication publication = db.Publications.Find(id);
            db.Publications.Remove(publication);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
