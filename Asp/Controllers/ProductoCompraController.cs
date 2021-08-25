using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;

namespace Asp.Controllers
{
    public class ProductoCompraController : Controller
    {
        [Authorize]
        // GET: ProductoCompra
        public ActionResult Index()
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.producto_compra.ToList());
            }
        }
        public static string NombreProducto(int idProduct)
        {
            using (var db = new inventario2021Entities())
            {
                return db.producto.Find(idProduct).nombre;
            }
        }

        public ActionResult ListaProductos()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.producto.ToList());
            }
        }

        public static int IdCompra(int idBuy)
        {
            using (var db = new inventario2021Entities())
            {
                return db.producto_compra.Find(idBuy).id_compra;
            }
        }

        public ActionResult ListaCompras()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.compra.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(producto_compra PdBuy)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.producto_compra.Add(PdBuy);
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

        public ActionResult Details(int id)
        {
            using (var db = new inventario2021Entities())
            {
                var findPdBuy = db.producto_compra.Find(id);
                return View(findPdBuy);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto_compra findPdBuy = db.producto_compra.Where(a => a.id == id).FirstOrDefault();
                    return View(findPdBuy);
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

        public ActionResult Edit(producto_compra editPdBuy)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto_compra Buy = db.producto_compra.Find(editPdBuy.id);

                    Buy.id = editPdBuy.id;
                    Buy.id_producto = editPdBuy.id_producto;
                    Buy.cantidad = editPdBuy.cantidad;

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

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto_compra Buy = db.producto_compra.Find(id);
                    db.producto_compra.Remove(Buy);

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
    }
}
