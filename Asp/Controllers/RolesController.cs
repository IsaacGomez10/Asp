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
    public class RolesController : Controller
    {
        [Authorize]
        // GET: Roles
        public ActionResult Index()
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.roles.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(roles rol)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using(var db = new inventario2021Entities())
                {
                    db.roles.Add(rol);
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

        public ActionResult Details(int id)
        {
            using(var db = new inventario2021Entities())
            {
                var findRol = db.roles.Find();
                return View(findRol);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    roles findRol = db.roles.Where(a => a.id == id).FirstOrDefault();
                    return View(findRol);
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

        public ActionResult Edit(roles editRol)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    roles rol = db.roles.Find(editRol.id);

                    rol.descripcion = editRol.descripcion;

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
                    var findRol = db.roles.Find(id);
                    db.roles.Remove(findRol);
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

                if(uplFile != null)
                {
                    string path = Server.MapPath("~/Uploads/Rol");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(uplFile.FileName);

                    string extension = Path.GetExtension(uplFile.FileName);

                    uplFile.SaveAs(filePath);

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach(string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            var newRol = new roles
                            {
                                descripcion = row
                            };

                            using (var db = new inventario2021Entities())
                            {
                                db.roles.Add(newRol);
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
                    var roles = db.roles.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistros)
                        .Take(cantidadRegistros).ToList();

                    var totalRegistros = db.roles.Count();
                    var modelo = new IndexViewModel();
                    modelo.Roles = roles;
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