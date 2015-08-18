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
    public class ObraController : Controller
    {
        private RecordFCSContext db = new RecordFCSContext();

        // GET: Obra
        public ActionResult Index()
        {
            return View(db.Obras.ToList());
        }

        // GET: Obra/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Obra obra = db.Obras.Find(id);
        //    if (obra == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(obra);
        //}

        // GET: Obra/Registrar
        public ActionResult Registrar()
        {
            var listaLetras = db.LetraFolios.Select(a => new { a.LetraFolioID, Nombre = a.Nombre + " - " + a.Descripcion, a.Status }).Where(a => a.Status).OrderBy(a => a.Nombre);
            var listaTipoObras = db.TipoObras.Select(a => new { a.TipoObraID, Nombre = a.Nombre + " - " + a.Descripcion, a.Status }).Where(a => a.Status).OrderBy(a => a.Nombre);
            ViewBag.LetraFolioID = new SelectList(listaLetras, "LetraFolioID", "Nombre", listaLetras.FirstOrDefault().LetraFolioID);
            ViewBag.TipoObraID = new SelectList(listaTipoObras, "TipoObraID", "Nombre");

            return View();
        }

        // POST: Obra/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = "ObraID,NumeroFolio,FechaRegistro,TipoObraID,LetraFolioID,Status,Temp")] Obra obra)
        {
            if (ModelState.IsValid)
            {
                obra.ObraID = Guid.NewGuid();
                db.Obras.Add(obra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LetraFolioID = new SelectList(db.LetraFolios, "LetraFolioID", "Nombre", obra.LetraFolioID);
            ViewBag.TipoObraID = new SelectList(db.TipoObras, "TipoObraID", "Nombre", obra.TipoObraID);
            return View(obra);
        }

        // GET: Obra/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obra obra = db.Obras.Find(id);
            if (obra == null)
            {
                return HttpNotFound();
            }
            ViewBag.LetraFolioID = new SelectList(db.LetraFolios, "LetraFolioID", "Nombre", obra.LetraFolioID);
            ViewBag.TipoObraID = new SelectList(db.TipoObras, "TipoObraID", "Nombre", obra.TipoObraID);
            return View(obra);
        }

        // POST: Obra/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ObraID,NumeroFolio,FechaRegistro,TipoObraID,LetraFolioID,Status,Temp")] Obra obra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LetraFolioID = new SelectList(db.LetraFolios, "LetraFolioID", "Nombre", obra.LetraFolioID);
            ViewBag.TipoObraID = new SelectList(db.TipoObras, "TipoObraID", "Nombre", obra.TipoObraID);
            return View(obra);
        }

        // GET: Obra/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obra obra = db.Obras.Find(id);
            if (obra == null)
            {
                return HttpNotFound();
            }
            return View(obra);
        }

        // POST: Obra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Obra obra = db.Obras.Find(id);
            db.Obras.Remove(obra);
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
