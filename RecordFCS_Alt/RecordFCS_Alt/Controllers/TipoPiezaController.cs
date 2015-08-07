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
    public class TipoPiezaController : BaseController
    {
        private RecordFCSContext db = new RecordFCSContext();

        public ActionResult Index()
        {
            //redireccionar a Tipo de Obras
            return RedirectToAction("Index", "TipoObra");
        }

        // GET: TipoPieza
        //root = true --> mostrar lista de piezas maestras
        public ActionResult Lista(Guid? id)
        {
            if (id == null) new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TipoObra tipoObra = db.TipoObras.Find(id);

            if (tipoObra == null) HttpNotFound();

            var lista = db.TipoPiezas.Where(a => a.TipoObraID == id && a.TipoPiezaPadreID == null).OrderBy(a => a.Orden);


            ViewBag.totalRegistros = lista.Count();
            ViewBag.id = id;
            ViewBag.nombre = tipoObra.Nombre;

            return PartialView("_Lista", lista.ToList());
        }

        // GET: TipoPieza/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoPieza tipoPieza = db.TipoPiezas.Find(id);
            if (tipoPieza == null)
            {
                return HttpNotFound();
            }
            return View(tipoPieza);
        }

        // GET: TipoPieza/Create
        public ActionResult Crear(Guid? id, bool principal = false)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TipoPieza tp = new TipoPieza();

            if (principal)
            {
                //id = TipoObraID
                TipoObra tipoObra = db.TipoObras.Find(id);
                if (tipoObra == null)
                    return HttpNotFound();

                tp.TipoObraID = tipoObra.TipoObraID;
                tp.Orden = tipoObra.TipoPiezas.Where(a => a.EsPrincipal).Count() + 1;
                tp.Status = true;
                tp.EsPrincipal = true;
            }
            else
            {
                //id = TipoPiezaPadreID
                TipoPieza tipoPiezaPadre = db.TipoPiezas.Find(id);
                if (tipoPiezaPadre == null)
                    return HttpNotFound();

                tp.TipoObraID = tipoPiezaPadre.TipoObraID;
                tp.Orden = tipoPiezaPadre.TipoPiezasHijas.Count + 1;
                tp.TipoPiezaPadreID = tipoPiezaPadre.TipoPiezaID;
                tp.Status = true;
                tp.EsPrincipal = false;
            }

            return PartialView("_Crear", tp);
        }

        // POST: TipoPieza/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "TipoPiezaID,Nombre,Descripcion,Prefijo,Orden,EsPrincipal,Status,TipoObraID,TipoPiezaPadreID,Temp")] TipoPieza tipoPieza)
        {
            if (ModelState.IsValid)
            {
                tipoPieza.TipoPiezaID = Guid.NewGuid();
                db.TipoPiezas.Add(tipoPieza);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoObraID = new SelectList(db.TipoObras, "TipoObraID", "Nombre", tipoPieza.TipoObraID);
            ViewBag.TipoPiezaPadreID = new SelectList(db.TipoPiezas, "TipoPiezaID", "Nombre", tipoPieza.TipoPiezaPadreID);
            return View(tipoPieza);
        }

        // GET: TipoPieza/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoPieza tipoPieza = db.TipoPiezas.Find(id);
            if (tipoPieza == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoObraID = new SelectList(db.TipoObras, "TipoObraID", "Nombre", tipoPieza.TipoObraID);
            ViewBag.TipoPiezaPadreID = new SelectList(db.TipoPiezas, "TipoPiezaID", "Nombre", tipoPieza.TipoPiezaPadreID);
            return View(tipoPieza);
        }

        // POST: TipoPieza/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoPiezaID,Nombre,Descripcion,Prefijo,Orden,EsPrincipal,Status,TipoObraID,TipoPiezaPadreID,Temp")] TipoPieza tipoPieza)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoPieza).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoObraID = new SelectList(db.TipoObras, "TipoObraID", "Nombre", tipoPieza.TipoObraID);
            ViewBag.TipoPiezaPadreID = new SelectList(db.TipoPiezas, "TipoPiezaID", "Nombre", tipoPieza.TipoPiezaPadreID);
            return View(tipoPieza);
        }

        // GET: TipoPieza/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoPieza tipoPieza = db.TipoPiezas.Find(id);
            if (tipoPieza == null)
            {
                return HttpNotFound();
            }
            return View(tipoPieza);
        }

        // POST: TipoPieza/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TipoPieza tipoPieza = db.TipoPiezas.Find(id);
            db.TipoPiezas.Remove(tipoPieza);
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
