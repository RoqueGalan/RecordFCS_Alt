using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecordFCS_Alt.Models;

namespace RecordFCS_Alt.Controllers
{
    public class PiezaController : Controller
    {
        private RecordFCSContext db = new RecordFCSContext();

        // GET: Pieza
        public ActionResult Index()
        {
            var piezas = db.Piezas.Include(p => p.Obra).Include(p => p.PiezaPadre).Include(p => p.TipoPieza).Include(p => p.Ubicacion);
            return View(piezas.ToList());
        }

        // GET: Pieza/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pieza pieza = db.Piezas.Find(id);
            if (pieza == null)
            {
                return HttpNotFound();
            }
            return View(pieza);
        }

        // GET: Pieza/Create
        public ActionResult Create()
        {
            ViewBag.ObraID = new SelectList(db.Obras, "ObraID", "Temp");
            ViewBag.PiezaPadreID = new SelectList(db.Piezas, "PiezaID", "SubFolio");
            ViewBag.TipoPiezaID = new SelectList(db.TipoPiezas, "TipoPiezaID", "Nombre");
            ViewBag.UbicacionID = new SelectList(db.Ubicaciones, "UbicacionID", "Nombre");
            return View();
        }

        // POST: Pieza/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PiezaID,SubFolio,FechaRegistro,Status,ObraID,TipoPiezaID,UbicacionID,PiezaPadreID,Temp")] Pieza pieza)
        {
            if (ModelState.IsValid)
            {
                pieza.PiezaID = Guid.NewGuid();
                db.Piezas.Add(pieza);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ObraID = new SelectList(db.Obras, "ObraID", "Temp", pieza.ObraID);
            ViewBag.PiezaPadreID = new SelectList(db.Piezas, "PiezaID", "SubFolio", pieza.PiezaPadreID);
            ViewBag.TipoPiezaID = new SelectList(db.TipoPiezas, "TipoPiezaID", "Nombre", pieza.TipoPiezaID);
            ViewBag.UbicacionID = new SelectList(db.Ubicaciones, "UbicacionID", "Nombre", pieza.UbicacionID);
            return View(pieza);
        }

        // GET: Pieza/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pieza pieza = db.Piezas.Find(id);
            if (pieza == null)
            {
                return HttpNotFound();
            }
            ViewBag.ObraID = new SelectList(db.Obras, "ObraID", "Temp", pieza.ObraID);
            ViewBag.PiezaPadreID = new SelectList(db.Piezas, "PiezaID", "SubFolio", pieza.PiezaPadreID);
            ViewBag.TipoPiezaID = new SelectList(db.TipoPiezas, "TipoPiezaID", "Nombre", pieza.TipoPiezaID);
            ViewBag.UbicacionID = new SelectList(db.Ubicaciones, "UbicacionID", "Nombre", pieza.UbicacionID);
            return View(pieza);
        }

        // POST: Pieza/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PiezaID,SubFolio,FechaRegistro,Status,ObraID,TipoPiezaID,UbicacionID,PiezaPadreID,Temp")] Pieza pieza)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pieza).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ObraID = new SelectList(db.Obras, "ObraID", "Temp", pieza.ObraID);
            ViewBag.PiezaPadreID = new SelectList(db.Piezas, "PiezaID", "SubFolio", pieza.PiezaPadreID);
            ViewBag.TipoPiezaID = new SelectList(db.TipoPiezas, "TipoPiezaID", "Nombre", pieza.TipoPiezaID);
            ViewBag.UbicacionID = new SelectList(db.Ubicaciones, "UbicacionID", "Nombre", pieza.UbicacionID);
            return View(pieza);
        }

        // GET: Pieza/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pieza pieza = db.Piezas.Find(id);
            if (pieza == null)
            {
                return HttpNotFound();
            }
            return View(pieza);
        }

        // POST: Pieza/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Pieza pieza = db.Piezas.Find(id);
            db.Piezas.Remove(pieza);
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
