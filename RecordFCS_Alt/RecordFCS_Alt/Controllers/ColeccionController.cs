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
    public class ColeccionController : BaseController
    {
        private RecordFCSContext db = new RecordFCSContext();

        // GET: Coleccion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            var lista = db.Colecciones.OrderBy(a => a.Nombre).ToList();

            ViewBag.totalRegistros = lista.Count;

            return PartialView("_Lista", lista);
        }

        // GET: Coleccion/Crear
        public ActionResult Crear()
        {

            var coleccion = new Coleccion()
            {
                Status = true
            };

            return PartialView("_Crear", coleccion);
        }

        // POST: Coleccion/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "ColeccionID,Nombre,Status,Temp")] Coleccion coleccion)
        {
            //validar el nombre
            var col = db.Colecciones.Select(a => new { a.Nombre, a.ColeccionID }).FirstOrDefault(a => a.Nombre == coleccion.Nombre);

            if (col != null)
                if (col.ColeccionID != coleccion.ColeccionID)
                    ModelState.AddModelError("Nombre", "Nombre ya existe.");

            if (ModelState.IsValid)
            {
                coleccion.ColeccionID = Guid.NewGuid();
                db.Colecciones.Add(coleccion);
                db.SaveChanges();

                AlertaSuccess(string.Format("Colección: <b>{0}</b> creada.", coleccion.Nombre), true);

                string url = Url.Action("Lista", "Coleccion");
                return Json(new { success = true, url = url });
            }

            return PartialView("_Crear", coleccion);
        }

        // GET: Coleccion/Editar/5
        public ActionResult Editar(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coleccion coleccion = db.Colecciones.Find(id);
            if (coleccion == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Editar", coleccion);
        }

        // POST: Coleccion/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "ColeccionID,Nombre,Status,Temp")] Coleccion coleccion)
        {
            //validar el nombre
            var col = db.Colecciones.Select(a => new { a.Nombre, a.ColeccionID }).FirstOrDefault(a => a.Nombre == coleccion.Nombre);

            if (col != null)
                if (col.ColeccionID != coleccion.ColeccionID)
                    ModelState.AddModelError("Nombre", "Nombre ya existe.");

            if (ModelState.IsValid)
            {
                db.Entry(coleccion).State = EntityState.Modified;
                db.SaveChanges();

                AlertaInfo(string.Format("Colección: <b>{0}</b> se editó.", coleccion.Nombre), true);
                string url = Url.Action("Lista", "Coleccion");
                return Json(new { success = true, url = url });
            }

            return PartialView("_Editar", coleccion);
        }

        // GET: Coleccion/Eliminar/5
        public ActionResult Eliminar(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coleccion coleccion = db.Colecciones.Find(id);
            if (coleccion == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Eliminar", coleccion);
        }

        // POST: Coleccion/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(Guid id)
        {
            string btnValue = Request.Form["accionx"];

            Coleccion coleccion = db.Colecciones.Find(id);

            switch (btnValue)
            {
                case "deshabilitar":
                    coleccion.Status = false;
                    db.Entry(coleccion).State = EntityState.Modified;
                    db.SaveChanges();
                    AlertaDefault(string.Format("Se deshabilito <b>{0}</b>", coleccion.Nombre), true);
                    break;
                case "eliminar":
                    db.Colecciones.Remove(coleccion);
                    db.SaveChanges();
                    AlertaDanger(string.Format("Se elimino <b>{0}</b>", coleccion.Nombre), true);
                    break;
                default:
                    AlertaDanger(string.Format("Ocurrio un error."), true);
                    break;
            }


            string url = Url.Action("Lista", "Coleccion");
            return Json(new { success = true, url = url });
        }

        public JsonResult EsUnico(string Nombre, Guid? ColeccionID)
        {
            bool x = false;

            var ubi = db.Colecciones.Select(a => new { a.ColeccionID, a.Nombre }).SingleOrDefault(a => a.Nombre == Nombre);

            x = ubi == null ? true : ubi.ColeccionID == ColeccionID ? true : false;

            return Json(x);
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
