using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Info_Net.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.Data.SqlClient;

namespace Info_Net.Controllers
{
    public class BillsController : Controller
    {
        private InfoNetContex db = new InfoNetContex();


		//REport
		//PDF
		public ActionResult PDF()
		{
			var report = this.GenerateBillsReport();
			var stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
			return File(stream, "application/pdf");
		}
		//WORD
		public ActionResult DOC()
		{
			var report = this.GenerateBillsReport();
			var stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
			return File(stream, "application/doc","Contisacion.doc");
		}

		private ReportClass GenerateBillsReport()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			var connection = new SqlConnection(connectionString);
			var dataTable = new DataTable();
			var sql = "Select * from Bills";

			try
			{
				connection.Open();
				var Comand = new SqlCommand(sql, connection);
				var adapter = new SqlDataAdapter(Comand);
				adapter.Fill(dataTable);
			}
			catch (Exception ex)
			{
				ex.ToString();
				throw;
			}

			var report = new ReportClass();
			report.FileName = Server.MapPath("/Reports/Bill.rpt");
			report.Load();
			report.SetDataSource(dataTable);
			return report;
		}

		// GET: Bills
		public ActionResult Index()
        {
            var bills = db.Bills.Include(b => b.Package);
            return View(bills.ToList());
        }

        // GET: Bills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: Bills/Create
        public ActionResult Create()
        {
            ViewBag.Package_id = new SelectList(db.Packages, "Package_id", "NombrePack");
            return View();
        }

        // POST: Bills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Bill_id,Nombres,Apaterno,Amaterno,Telefono,Email,Package_id,Calle,NumEx,Estado,Municipio,colonia")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Bills.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Package_id = new SelectList(db.Packages, "Package_id", "NombrePack", bill.Package_id);
            return View(bill);
        }

        // GET: Bills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.Package_id = new SelectList(db.Packages, "Package_id", "NombrePack", bill.Package_id);
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Bill_id,Nombres,Apaterno,Amaterno,Telefono,Email,Package_id,Calle,NumEx,Estado,Municipio,colonia")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Package_id = new SelectList(db.Packages, "Package_id", "NombrePack", bill.Package_id);
            return View(bill);
        }

        // GET: Bills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
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
