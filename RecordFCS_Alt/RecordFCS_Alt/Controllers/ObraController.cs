using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecordFCS_Alt.Models;
using System.Threading;
using System.Text.RegularExpressions;

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


        public int DarFolioValido(int LetraFolioID, int Numero, bool segunda = false)
        {
            int timeDormirMiliSeg = 2000; //2 segundo
            Thread.Sleep(timeDormirMiliSeg);

            var existe = true;

            int i = Numero - 1 <= 0 ? 0 : Numero - 1;
            int? temp = null;

            do
            {
                i++;
                temp = db.Obras.Where(a => a.LetraFolioID == LetraFolioID && a.NumeroFolio == i).Select(a => a.NumeroFolio).FirstOrDefault();

                //cuando num = 0 es por que no existe 
                existe = temp == null || temp == 0 ? false : true;

            } while (existe);

            //revalidar 2da vez 
            if (!segunda)
                Numero = DarFolioValido(LetraFolioID, i, true);
            else
                Numero = i;

            return Numero;
        }

        // POST: Obra/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Guid? TipoObraID, int? LetraFolioID, Guid? TipoPiezaID)
        {
            var Formulario = Request.Form;


            if (TipoObraID == null || LetraFolioID == null || TipoPiezaID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var letra = db.LetraFolios.Find(LetraFolioID);
            var tipoObra = db.TipoObras.Find(TipoObraID);
            var tipoPieza = tipoObra.TipoPiezas.FirstOrDefault(a => a.TipoPiezaID == TipoPiezaID);

            if (tipoObra == null || letra == null || tipoPieza == null)
                return HttpNotFound();



            var obra = new Obra()
            {
                FechaRegistro = DateTime.Now,
                TipoObraID = tipoObra.TipoObraID,
                LetraFolioID = letra.LetraFolioID,
                Status = false,
                NumeroFolio = 1
            };

            obra.ObraID = Guid.NewGuid();




            //validar el numero de folio
            obra.NumeroFolio = DarFolioValido(obra.LetraFolioID, obra.NumeroFolio);

            //Guardar la obra
            //db.Obras.Add(obra);
            //db.SaveChanges();

            //Crear pieza
            Pieza pieza = new Pieza()
            {
                FechaRegistro = obra.FechaRegistro,
                ObraID = obra.ObraID,
                Status = false,
                PiezaPadreID = null, // null = Principal o Maestra
                TipoPiezaID = tipoPieza.TipoPiezaID,
                SubFolio = tipoPieza.Prefijo
            };

            pieza.PiezaID = Guid.NewGuid();

            //Guardar la pieza
            //db.Piezas.Add(pieza);
            //db.SaveChanges();

            //lista de atributos de registro
            var listaAttRegistro = tipoPieza.Atributos.Where(a => a.Status && a.MostrarAtributos.Any(b => b.TipoMostrar.Nombre == "Registro" && b.Status) && a.TipoAtributo.Status).OrderBy(a => a.Orden).ToList();


            List<AtributoPieza> listaAdd_AttGen = new List<AtributoPieza>();
            List<AutorPieza> listaAdd_AttAutor = new List<AutorPieza>();
            List<ImagenPieza> listaAdd_AttImg = new List<ImagenPieza>();
            List<TecnicaPieza> listaAdd_AttTec = new List<TecnicaPieza>();
            List<MedidaPieza> listaAdd_AttMed = new List<MedidaPieza>();
            Ubicacion ubicacionAdd = null;

            /*
             * Extraer los registros del formulario dependiendo el tipo de Atributo
             * 
             
             * AUTOR
             *      MULTIPLE
             *              id_####################_#################### (Input)
             *              
             * IMAGEN
             *      SIMPLE
             *          id_####################_File        (File)
             *          id_####################_Titulo      (Input)
             *          id_####################_Descripcion (Input)
             *          
             * TECNICA
             *      SIMPLE
             *          id_####################     (Select)
             *          
             * 
             * TIPO MEDIDA
             *      SIMPLE
             *          id_####################                 (Select)(TipoMedida)
             *          id_####################_UML             (Select)
             *          id_####################_Altura          (input)
             *          id_####################_Anchura         (input)
             *          id_####################_Profundidad     (input)
             *          id_####################_Diametro        (input)
             *          id_####################_Diametro2       (input)
             * 
             * 
             * 
             * UBICACION
             *      SIMPLE  
             *          id_####################     (select)
             *
             */





            foreach (var att in listaAttRegistro)
            {
                var tipoAtt = att.TipoAtributo;

                if (tipoAtt.EsGenerico)
                {
                    /*
                     * GENERICO
                     *      LISTA
                     *          SIMPLE
                     *              id_#################### (Select)
                     *          MULTI
                     *              id_####################_#################### (Input)
                     */
                    if (tipoAtt.EsLista)
                    {
                        List<string> listaKey;

                        if (tipoAtt.EsMultipleValor)
                            listaKey = Formulario.AllKeys.Where(k => k.StartsWith("id_" + att.AtributoID + "_")).ToList();
                        else
                            listaKey = Formulario.AllKeys.Where(k => k.StartsWith("id_" + att.AtributoID)).ToList();

                        //buscar en form todas las llaves que correspondan al id_xxxxxxxxxxxxxx_xxxxxxxxxxxxxx
                        foreach (string key in listaKey)
                        {
                            var addOk = true;
                            string valor = Formulario[key];


                            addOk = String.IsNullOrWhiteSpace(valor) ? false : true;

                            //validar el valorID, buscar el valor
                            Guid valorID = addOk ? new Guid(valor) : new Guid(new Byte[16]);


                            addOk = !addOk ? addOk : listaAdd_AttGen.Where(a => a.AtributoID == att.AtributoID && a.ListaValorID == valorID).FirstOrDefault() == null ? true : false;

                            addOk = !addOk ? addOk : db.ListaValores.Where(a => a.TipoAtributoID == tipoAtt.TipoAtributoID && a.Status && a.ListaValorID == valorID).FirstOrDefault() == null ? false : true;

                            if (addOk)
                                listaAdd_AttGen.Add(new AtributoPieza()
                                {
                                    AtributoPiezaID = Guid.NewGuid(),
                                    AtributoID = att.AtributoID,
                                    PiezaID = pieza.PiezaID,
                                    Status = true,
                                    ListaValorID = valorID
                                });
                        }
                    }
                    else
                    {
                        /*
                         * GENERICO
                         *    CAMPO
                         *        SIMPLE  
                         *            id_#################### (Input)
                         *        MULTI   
                         *            id_####################_##### (Input)
                         */

                        List<string> listaKey;

                        if (tipoAtt.EsMultipleValor)
                            listaKey = Formulario.AllKeys.Where(k => k.StartsWith("id_" + att.AtributoID + "_")).ToList();
                        else
                            listaKey = Formulario.AllKeys.Where(k => k.StartsWith("id_" + att.AtributoID)).ToList();


                        //buscar en form todas las llaves que correspondan al id_xxxxxxxxxxxxxx
                        foreach (string key in listaKey)
                        {
                            var addOk = true;
                            string valor = Formulario[key];

                            //validar el campo, quitar espacios en blanco, bla bla bla
                            valor = valor.Trim(); // quitar espacios en inicio y fin
                            valor = Regex.Replace(valor, @"\s+", " "); //quitar espacios de sobra

                            addOk = String.IsNullOrWhiteSpace(valor) ? false : true;
                            addOk = !addOk ? addOk : listaAdd_AttGen.Where(a => a.AtributoID == att.AtributoID && a.Valor == valor).FirstOrDefault() == null ? true : false;

                            if (addOk)
                                listaAdd_AttGen.Add(new AtributoPieza()
                                {
                                    AtributoPiezaID = Guid.NewGuid(),
                                    AtributoID = att.AtributoID,
                                    PiezaID = pieza.PiezaID,
                                    Status = true,
                                    Valor = valor
                                });

                        }
                    }
                }
                else
                {
                    switch (tipoAtt.TablaSQL)
                    {
                        case "Autor":
                            //filtrar id_#######
                            List<string> listaKey = Formulario.AllKeys.Where(k => k.StartsWith("id_" + att.AtributoID + "_")).ToList();

                            ///filtrar: ignorar los _prefijo
                            listaKey = listaKey.Where(k => !k.EndsWith("_prefijo")).ToList();

                            //buscar en form todas las llaves que correspondan al id_xxxxxxxxxxxxxx_xxxxxxxxxxxxxx
                            foreach (string key in listaKey)
                            {
                                var addOk = true;
                                string text_autorID = Formulario[key];
                                string text_prefijo = Formulario[key + "_prefijo"];

                                addOk = String.IsNullOrWhiteSpace(text_autorID) ? false : true;

                                //validar el valorID, buscar el valor
                                Guid autorID = addOk ? new Guid(text_autorID) : new Guid(new Byte[16]);

                                addOk = !addOk ? addOk : listaAdd_AttAutor.Where(a => a.AutorID == autorID).FirstOrDefault() == null ? true : false;

                                addOk = !addOk ? addOk : db.Autores.Where(a => a.Status && a.AutorID == autorID).FirstOrDefault() == null ? false : true;

                                if (addOk)
                                {
                                    var autorPieza = new AutorPieza()
                                    {
                                        AutorID = autorID,
                                        PiezaID = pieza.PiezaID,
                                        esPrincipal = false,
                                        Prefijo = text_prefijo,
                                        Status = true
                                    };

                                    //validar si es principal
                                    if (autorPieza.Prefijo.ToLower() == "principal")
                                        autorPieza.esPrincipal = listaAdd_AttAutor.Where(a => a.esPrincipal).Count() == 0 ? true : false;

                                    listaAdd_AttAutor.Add(autorPieza);
                                }
                            }
                            break;

                        case "Ubicacion":

                            break;

                        case "TipoTecnica":

                            break;

                        case "TipoMedida":

                            break;

                        case "ImagenPieza":

                            break;

                        default:
                            break;
                    }

                }



            }



            if (ModelState.IsValid)
            {
                obra.ObraID = Guid.NewGuid();
                db.Obras.Add(obra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var listaLetras = db.LetraFolios.Select(a => new { a.LetraFolioID, Nombre = a.Nombre + " - " + a.Descripcion, a.Status }).Where(a => a.Status).OrderBy(a => a.Nombre);
            var listaTipoObras = db.TipoObras.Select(a => new { a.TipoObraID, Nombre = a.Nombre + " - " + a.Descripcion, a.Status }).Where(a => a.Status).OrderBy(a => a.Nombre);
            ViewBag.LetraFolioID = new SelectList(listaLetras, "LetraFolioID", "Nombre", obra.LetraFolioID);
            ViewBag.TipoObraID = new SelectList(listaTipoObras, "TipoObraID", "Nombre", obra.TipoObraID);

            return View(obra);
        }

        // GET: Obra/Editar/5
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

        // POST: Obra/Editar/5
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

        // GET: Obra/Eliminar/5
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

        // POST: Obra/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
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
