using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;
using Rotativa;
using System.IO;

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

        //Copia que se Descargara
        public ActionResult ReportePdf()
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

        public ActionResult Descarga()
        {
            return new ActionAsPdf("ReportePdf") { FileName = "Reporte_Producto.pdf" };
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
                    string path = Server.MapPath("~/UploadsProduct/");

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
                            var newProduct = new producto
                            {
                                nombre = row.Split(';')[0],
                                percio_unitario = Convert.ToInt32( row.Split(';')[1]),
                                descripcion = row.Split(';')[2],
                                cantidad = Convert.ToInt32(row.Split(';')[3]),
                                id_proveedor = Convert.ToInt32(row.Split(';')[4])

                            };

                            using (var db = new inventario2021Entities())
                            {
                                db.producto.Add(newProduct);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }
    }
}