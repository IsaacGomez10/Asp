using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;
using System.Web.Security;
using System.Text;
using System.IO;
using System.Web.Routing;


namespace Asp.Controllers
{
    public class UsuarioController : Controller
    {
        //private object DataTime;

        [Authorize]
        // GET: Usuario
        // Conexión a la base de datos
        public ActionResult Index()
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.usuario.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(usuario usuario)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using(var db = new inventario2021Entities())
                {
                    usuario.password = UsuarioController.HashSHA1(usuario.password);
                    db.usuario.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "Error" + ex);
                return View();
            }

        }
    
        public static string HashSHA1(string value)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for(var i = 0; i<hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public ActionResult Details(int id)
        {
            using(var db = new inventario2021Entities())
            {
                var findUser = db.usuario.Find(id);
                return View(findUser);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    usuario findUser = db.usuario.Where(a => a.id == id).FirstOrDefault();
                    return View(findUser);
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

        public ActionResult Edit(usuario editUser)
        {
            try
            {
                using(var db = new inventario2021Entities())
                {
                    usuario user = db.usuario.Find(editUser.id);

                    user.nombre = editUser.nombre;
                    user.apellido = editUser.apellido;
                    user.email = editUser.email;
                    user.fecha_nacimiento = editUser.fecha_nacimiento;
                    user.password = UsuarioController.HashSHA1(editUser.password);

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
                    var findUser = db.usuario.Find(id);
                    db.usuario.Remove(findUser);
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

        public ActionResult Login(string mensaje = "")
        {
            ViewBag.Message = mensaje;
            return View();
        }

        [HttpPost]
        [ ValidateAntiForgeryToken]

        public ActionResult Login(string user, string password)
        {
            try
            {
                string passEncrip = UsuarioController.HashSHA1(password);
                using(var db = new inventario2021Entities())
                {
                    var userLogin = db.usuario.FirstOrDefault(e => e.email == user && e.password == passEncrip);
                    if(userLogin != null)
                    {
                        FormsAuthentication.SetAuthCookie(userLogin.email, true);
                        Session["User"] = userLogin;

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Login("Verifique sus datos");
                    }
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error " + ex);
                return View();
            }
        }

         [Authorize]

        public ActionResult CloseSession()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
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

                    string path = Server.MapPath("~/UploadsUser/");

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
                            var newUser = new usuario
                            {
                                nombre = row.Split(';')[0],
                                apellido = row.Split(';')[1],
                                fecha_nacimiento = Convert.ToDateTime(row.Split(';')[2]),
                                email = row.Split(';')[3],
                                password = row.Split(';')[4]
                            };

                            using (var db = new inventario2021Entities())
                            {
                                db.usuario.Add(newUser);
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
    }
}