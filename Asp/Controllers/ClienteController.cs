using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;

namespace Asp.Controllers
{
    public class ClienteController : Controller
    {
        [Authorize]
        // GET: Cliente
        public ActionResult Index()
        {
            using(var db = new inventario2021Entities())
            {
                return View(db.cliente.ToList());
            }
            
        }
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