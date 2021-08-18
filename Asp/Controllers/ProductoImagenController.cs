using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;

namespace Asp.Controllers
{
    public class ProductoImagenController : Controller
    {
        // GET: ProducImg
        public ActionResult Index()
        {
            using(var db = new inventario2021Entities())
            {
                return View(db.producto_imagen.ToList());
            }
        }

        public static string NombreProducto(int idProduct)
        {
            using (var db = new inventario2021Entities())
            {
                return db.proveedor.Find(idProduct).nombre;
            }
        }

        public ActionResult ListaProductos()
        {
            using(var db = new inventario2021Entities())
            {
                return PartialView(db.producto.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(producto_imagen productImg)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.producto_imagen.Add(productImg);
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

        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto_imagen findProducImg = db.producto_imagen.Where(a => a.id == id).FirstOrDefault();
                    return View(findProducImg);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using(var db = new inventario2021Entities())
            {
                var findProductImg = db.producto_imagen.Find(id);
                return View(findProductImg);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(producto_imagen editProcImg)
        {
            try
            {
                using(var db = new inventario2021Entities())
                {
                    producto_imagen productImg = db.producto_imagen.Find(editProcImg.id);

                    productImg.imagen = editProcImg.imagen;
                    productImg.id_producto = editProcImg.id_producto;

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
                    producto_imagen productImg = db.producto_imagen.Find(id);
                    db.producto_imagen.Remove(productImg);

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
    }
}