using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;
using System.IO;

namespace Asp.Controllers
{
    public class ProveedorController : Controller
    {
        [Authorize]
        // GET: Proveedor
        public ActionResult Index()
        {

            using (var db = new inventario2021Entities())
            {
                return View(db.proveedor.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]

        public ActionResult Create(proveedor provider)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.proveedor.Add(provider);
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
            using (var db = new inventario2021Entities())
            {
                var findProvider = db.proveedor.Find(id);
                return View(findProvider);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var findProvider = db.proveedor.Find(id);
                    db.proveedor.Remove(findProvider);
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
                    proveedor findProvider = db.proveedor.Where(a => a.id == id).FirstOrDefault();
                    return View(findProvider);
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

        public ActionResult Edit(proveedor editProvider)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    proveedor provider = db.proveedor.Find(editProvider.id);

                    provider.nombre = editProvider.nombre;
                    provider.direccion = editProvider.direccion;
                    provider.telefono = editProvider.telefono;
                    provider.nombre_contacto = editProvider.nombre_contacto;

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

        public ActionResult uploadCSV()
        {
            return View();
        }

        [HttpPost]

        public ActionResult uploadCSV(HttpPostedFileBase fileForm)
        {
            try
            {
                //string para guardar la ruta
                string filePath = string.Empty;

                //condicion para saber si el archivo llego
                if (fileForm != null)
                {
                    //ruta de la carpeta que guardara el archivo
                    string path = Server.MapPath("~/Uploads/");

                    //condicion para saber si la carpeta uploads existe
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //obtener nombre del archivo
                    filePath = path + Path.GetFileName(fileForm.FileName);

                    //obtener la extencion del archivo
                    string extension = Path.GetExtension(fileForm.FileName);

                    //guardar el archivo
                    fileForm.SaveAs(filePath);

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            var newProveedor = new proveedor
                            {
                                nombre = row.Split(';')[0],
                                direccion = row.Split(';')[1],
                                telefono = row.Split(';')[2],
                                nombre_contacto = row.Split(';')[3]
                            };

                            using (var db = new inventario2021Entities())
                            {
                                db.proveedor.Add(newProveedor);
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