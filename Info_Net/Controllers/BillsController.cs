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
using CrystalDecisions.Shared;

namespace Info_Net.Controllers
{
    public class BillsController : Controller
    {
        private InfoNetContex db = new InfoNetContex();
		private Reports.BillsReport crBillsReport { get; set; }
		
		//REport
		//PDF
		public ActionResult PDF()
		{
			var report = this.GenerateBillsReport();
			var stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
			return File(stream, "application/pdf");
			/*
			 * ExportOptions exportOptions = crProducto.ExportOptions;
                    DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
                    CrystalDecisions.Shared.PdfRtfWordFormatOptions formatTypeOptions = new PdfRtfWordFormatOptions();
                    //destino
                    diskFileDestinationOptions.DiskFileName = saveFileDialog1.FileName;
                    exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    exportOptions.DestinationOptions = diskFileDestinationOptions;
                    exportOptions.FormatOptions = formatTypeOptions;
			 */
		}
		//WORD
		public ActionResult DOC()
		{
			//this.crBillsReport = new Reports.BillsReport();
			//crBillsReport.Viewcar
			var report = this.GenerateBillsReport();//-> Esto para que es?

			// es un seb proceso del crystal para la exportacion del reporte al formtao que se le asigna por medio de la clase
			//ReportClass GenerateBillsReport() que es la que se encarga de traer lso datos y barir la coneccion a la bd mm ya veo
			// pero puedes buscar otras alternativas 

			var stream = crBillsReport.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
			return File(stream, "application/doc", "Cotisacion.xls");
			
		}

		private ReportClass GenerateBillsReport()
		{
			this.crBillsReport = new Reports.BillsReport();
			Reports.DtsBill isDts = new Reports.DtsBill();
			var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			var connection = new SqlConnection(connectionString);
			var dataTable = new DataTable();
			var sql = "Select * from Bills";
			var milst = new List<Bill>();
			try
			{
				connection.Open();

				var Comand = new SqlCommand(sql, connection);
				SqlDataReader miRead = Comand.ExecuteReader();
				if (miRead.HasRows)
				{
					while (miRead.Read())
					{
						Bill isBillObjet = new Bill();
						isBillObjet.Bill_id = miRead.GetInt32(0);
						isBillObjet.Nombres = miRead.GetString(1);
						isBillObjet.Apaterno = miRead.GetString(2);
						milst.Add(isBillObjet);
					}
				}

				foreach (Bill item in milst)
				{
					isDts.DtBill.AddDtBillRow(item.Bill_id.ToString(), item.Nombres.ToString(),item.Apaterno.ToString());
				}

				//var adapter = new SqlDataAdapter(Comand);
				//adapter.Fill(dataTable);
			}
			catch (Exception ex)
			{
				ex.ToString();
				throw;
			}

			//var report = new ReportClass();
			crBillsReport.FileName = Server.MapPath("/Reports/BillsReport.rpt");
			crBillsReport.Load();
			crBillsReport.SetDataSource(isDts);

			









			ExportOptions exportOptions = crBillsReport.ExportOptions;
			DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
			CrystalDecisions.Shared.PdfRtfWordFormatOptions formatTypeOptions = new PdfRtfWordFormatOptions();
			//destino
			diskFileDestinationOptions.DiskFileName = "MiReporte";
			exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
			exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
			exportOptions.DestinationOptions = diskFileDestinationOptions;
			exportOptions.FormatOptions = formatTypeOptions;
			crBillsReport.Export();//
			//File(crBillsReport.Export(), "application/doc",)
			//var stream = crBillsReport.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
			//return File File(stream, "application/doc", "Cotisacion.xls");
			return crBillsReport;
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
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
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
