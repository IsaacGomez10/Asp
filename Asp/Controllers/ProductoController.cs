using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;
using Rotativa;

namespace Asp.Controllers
{
    public class ProductoController : Controller
    {
        [Authorize]
        // GET: Producto
        public ActionResult Index()
        {
            using(var db = new inventario2021Entities())
            {
                return View(db.producto.ToList());
            }
        }

        public static string NombreProveedor(int idProvider)
        {
            using (var db = new inventario2021Entities())
            {
                return db.proveedor.Find(idProvider).nombre;
            }
        }

        public ActionResult ListaProveedores()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.proveedor.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(producto product)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.producto.Add(product);
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
                    producto findProduct = db.producto.Where(a => a.id == id).FirstOrDefault();
                    return View(findProduct);
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
            using (var db = new inventario2021Entities())
            {
                var findProduct = db.producto.Find(id);
                return View(findProduct);
            }
        }

         [HttpPost]
         [ValidateAntiForgeryToken]

         public ActionResult Edit(producto editProduct)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto product = db.producto.Find(editProduct.id);

                    product.nombre = editProduct.nombre;
                    product.percio_unitario = editProduct.percio_unitario;
                    product.descripcion = editProduct.descripcion;
                    product.cantidad = editProduct.cantidad;
                    product.id_proveedor = editProduct.id_proveedor;

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
                using (var db = new inventario2021Entities())
                {
                    producto product = db.producto.Find(id);
                    db.producto.Remove(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error" + ex);
                return View();
            }
        }

        public ActionResult Reporte()
        {
            try
            {
                var db = new inventario2021Entities();
                var query = from tabProveedor in db.proveedor
                            join tabProducto in db.producto on tabProveedor.id equals tabProducto.id_proveedor
                            select new Reporte
                            {
                                nombreProveedor = tabProveedor.nombre,
                                telefonoProveedor = tabProveedor.telefono,
                                direccionProveedor = tabProveedor.direccion,
                                nombreProducto = tabProducto.nombre,
                                precioProducto = tabProducto.percio_unitario
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
            return new ActionAsPdf("Reporte") { FileName = "Reporte_Producto.pdf" };
        }
    }
}