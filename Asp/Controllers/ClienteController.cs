using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;
using System.IO;
using System.Web.Routing;

namespace Asp.Controllers
{
    public class ClienteController : Controller
    {
        [Authorize]
        // GET: Cliente

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(cliente cliente)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.cliente.Add(cliente);
                    db.SaveChanges();
                    return RedirectToAction("PagIndex");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error" + ex);
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (var db = new inventario2021Entities())
            {
                var findCliente = db.cliente.Find(id);
                return View(findCliente);
            }

        }

        public ActionResult Delete(int id)
        {


            try
            {
                using (var db = new inventario2021Entities())
                {
                    var findCliente = db.cliente.Find(id);
                    db.cliente.Remove(findCliente);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }

        }

        public ActionResult Edit(int id)
        {

            try
            {
                using (var db = new inventario2021Entities())
                {
                    cliente findClient = db.cliente.Where(a => a.id == id).FirstOrDefault();
                    return View(findClient);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(cliente editCliente)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    cliente Client = db.cliente.Find(editCliente.id);

                    Client.nombre = editCliente.nombre;
                    Client.documento = editCliente.documento;
                    Client.email = editCliente.email;

                    db.SaveChanges();
                    return RedirectToAction("PagIndex");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Cargar()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Cargar(HttpPostedFileBase uplFile)
        {
            try
            {

                string filePath = string.Empty;

                if (uplFile != null)
                {
                    string path = Server.MapPath("~/UploadsClient");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(uplFile.FileName);

                    string extension = Path.GetExtension(uplFile.FileName);

                    uplFile.SaveAs(filePath);

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            var newCliente = new cliente
                            {
                                nombre = row.Split(';')[0],
                                documento = row.Split(';')[1],
                                email = row.Split(';')[2]
                            };

                            using(var db = new inventario2021Entities())
                            {
                                db.cliente.Add(newCliente);
                                db.SaveChanges();
                            }

                        }
                    }
                }

                return View();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult PagIndex(int pagina = 1)
        {
            try
            {
                var cantidadRegistros = 5;

                using (var db = new inventario2021Entities())
                {
                    var cliente = db.cliente.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistros)
                        .Take(cantidadRegistros).ToList();

                    var totalRegistros = db.cliente.Count();
                    var modelo = new IndexViewModel();
                    modelo.Clientes = cliente;
                    modelo.paginaActual = pagina;
                    modelo.totalRegistros = totalRegistros;
                    modelo.registrosPorPagina = cantidadRegistros;
                    modelo.valueQueryString = new RouteValueDictionary();

                    return View(modelo);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

    }
}