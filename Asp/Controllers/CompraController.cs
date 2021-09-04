using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;
using Rotativa;

namespace Asp.Controllers
{
    public class CompraController : Controller
    {
        [Authorize]
        // GET: Compra
        public ActionResult Index()
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.compra.ToList());
            }
        }

        public static string NombreCliente(int idClient)
        {
            using (var db = new inventario2021Entities())
            {
                return db.cliente.Find(idClient).nombre;
            }
        }
        
        public ActionResult ListaClientes()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.cliente.ToList());
            }
        }

        public static string NombreUsuario(int idUser)
        {
            using (var db = new inventario2021Entities())
            {
                return db.usuario.Find(idUser).nombre;
            }
        }

        public ActionResult ListaUsuarios()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.usuario.ToList());
            }
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(compra Buy)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.compra.Add(Buy);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error" + ex);
                return View();
            }



        }

        public ActionResult Details(int id)
        {
            using(var db = new inventario2021Entities())
            {
                var findBuy = db.compra.Find(id);
                return View(findBuy);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using(var db = new inventario2021Entities())
                {
                    compra findBuy = db.compra.Where(a => a.id == id).FirstOrDefault();
                    return View(findBuy);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(compra editCompra)
        {
            try
            {
                using(var db = new inventario2021Entities())
                {
                    compra buy = db.compra.Find(editCompra.id);

                    buy.fecha = editCompra.fecha;
                    buy.total = editCompra.total;
                    buy.id_cliente = editCompra.id_cliente;
                    buy.id_usuario = editCompra.id_usuario;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using(var db = new inventario2021Entities())
                {
                    compra buy = db.compra.Find(id);
                    db.compra.Remove(buy);

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error" + ex);
                return View();
            }
        }

        public ActionResult ReporteCompra()
        {
            try
            {
                var db = new inventario2021Entities();
                var query = from tabCompra in db.compra
                            join tabCliente in db.cliente on tabCompra.id_cliente equals tabCliente.id
                            join tabProductComp in db.producto_compra on tabCompra.id equals tabProductComp.id_compra
                            join tabUsuario in db.usuario on tabCompra.id_usuario equals tabUsuario.id
                            orderby tabCompra.fecha ascending
                            select new ReporteCompra
                            {
                                
                                nombreCliente = tabCliente.nombre,
                                cantidadProdctComp = tabProductComp.cantidad,
                                nombreUsuario = tabUsuario.nombre,
                                fechaCompra = tabCompra.fecha,
                                totalCompra = tabCompra.total


                            };

                return View(query);

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Descarga()
        {
            return new ActionAsPdf("ReporteCompra") { FileName = "Reporte_Compra.pdf" };
        }
    }
}