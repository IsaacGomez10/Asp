using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;

namespace Asp.Controllers
{
    public class UsuarioRolController : Controller
    {
        [Authorize]
        // GET: UsuarioRol
        public ActionResult Index()
        {
            using(var db = new inventario2021Entities())
            {
                return View(db.usuariorol.ToList());
            }
        }

        public static string NombreUsuario(int idUser)
        {
            using(var db = new inventario2021Entities())
            {
                return db.usuario.Find(idUser).nombre;
            }
        }

        public ActionResult ListaUsuarios()
        {
            using(var db = new inventario2021Entities())
            {
                return PartialView(db.usuario.ToList());
            }
        }

        public static string NombreRol(int idRol)
        {
            using(var db = new inventario2021Entities())
            {
                return db.roles.Find(idRol).descripcion;
            }
        }

        public ActionResult DescripcionRol()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.roles.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create (usuariorol newUserRol)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using(var db = new inventario2021Entities())
                {
                    db.usuariorol.Add(newUserRol);
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

        public ActionResult Details(int id)
        {
            using(var db = new inventario2021Entities())
            {
                var findUserRol = db.usuariorol.Find(id);
                return View(findUserRol);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    usuariorol findUserRol = db.usuariorol.Where(a => a.id == id).FirstOrDefault();
                    return View(findUserRol);
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

        public ActionResult Edit(usuariorol editUserRol)
        {
            try
            {
                using(var db = new inventario2021Entities())
                {
                    usuariorol rol = db.usuariorol.Find(editUserRol.id);

                    rol.idRol = editUserRol.idRol;
                    rol.idUsuario = editUserRol.idUsuario;

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

        public ActionResult Delete (int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    usuariorol rol = db.usuariorol.Find(id);
                    db.usuariorol.Remove(rol);

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