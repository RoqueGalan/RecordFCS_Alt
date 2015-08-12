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
    public class TipoAtributoController : BaseController
    {
        private RecordFCSContext db = new RecordFCSContext();

        // GET: TipoAtributo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            var lista = db.TipoAtributos.OrderBy(a => a.Nombre).ToList();

            ViewBag.totalRegistros = lista.Count;

            return PartialView("_Lista", lista);
        }

        //// GET: TipoAtributo/Detalles/5
        //public ActionResult Detalles(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TipoAtributo tipoAtributo = db.TipoAtributos.Find(id);
        //    if (tipoAtributo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tipoAtributo);
        //}

        // GET: TipoAtributo/Crear
        public ActionResult Crear()
        {
            //Valores default
            var tipoAtt = new TipoAtributo()
            {
                DatoCS = "string",
                EsGenerico = true,
                EnBuscador = false,
                Orden = db.TipoAtributos.Count() + 1,
                EsLista = false,
                EsMultipleValor = false,
                PerteneceA = "Pieza",
                TablaSQL = "ListaValor",
                Status = true
            };

            return PartialView("_Crear", tipoAtt);
        }

        // POST: TipoAtributo/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "TipoAtributoID,Nombre,Descripcion,Dato,EsLista,EsMultipleValor,MyProperty,PerteneceA,Tabla,HTMLAtributos,EstaEnBuscador,Orden,Status,Temp")] TipoAtributo tipoAtt)
        {
            //revalidar campo unico

            if (ModelState.IsValid)
            {
                tipoAtt.TipoAtributoID = Guid.NewGuid();
                db.TipoAtributos.Add(tipoAtt);
                db.SaveChanges();

                AlertaSuccess(string.Format("Tipo de Atributo: <b>{0}</b> creado.", tipoAtt.Nombre), true);

                string url = Url.Action("Lista", "TipoAtributo");
                return Json(new { success = true, url = url });
            }

            return PartialView("_Crear", tipoAtt);
        }

        // GET: TipoAtributo/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoAtributo tipoAtt = db.TipoAtributos.Find(id);
            if (tipoAtt == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Crear", tipoAtt);
        }

        // POST: TipoAtributo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoAtributoID,Nombre,Descripcion,Dato,EsLista,EsMultipleValor,MyProperty,PerteneceA,Tabla,HTMLAtributos,EstaEnBuscador,Orden,Status,Temp")] TipoAtributo tipoAtributo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoAtributo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoAtributo);
        }

        // GET: TipoAtributo/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoAtributo tipoAtributo = db.TipoAtributos.Find(id);
            if (tipoAtributo == null)
            {
                return HttpNotFound();
            }
            return View(tipoAtributo);
        }

        // POST: TipoAtributo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TipoAtributo tipoAtributo = db.TipoAtributos.Find(id);
            db.TipoAtributos.Remove(tipoAtributo);
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
