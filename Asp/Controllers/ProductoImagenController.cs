using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Models;
using System.IO;

namespace Asp.Controllers
{
    public class ProductoImagenController : Controller
    {
        // GET: ProductoImagen
        public ActionResult Index()
        {
            return View();
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

        public ActionResult CargarImagen()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CargarImagen(int producto, HttpPostedFileBase imagen)
        {
            try
            {
                string filePath = string.Empty;
                string nameFile = "";


                if (imagen != null)
                {

                    string path = Server.MapPath("~/UploadsPictures/");


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    nameFile = Path.GetFileName(imagen.FileName);


                    filePath = path + Path.GetFileName(imagen.FileName);


                    string extension = Path.GetExtension(imagen.FileName);


                    imagen.SaveAs(filePath);

                    using (var db = new inventario2021Entities())
                    {
                        var imagenProducto = new producto_imagen();
                        imagenProducto.id_producto = producto;
                        imagenProducto.imagen = "/UploadsPictures/" + nameFile;
                        db.producto_imagen.Add(imagenProducto);
                        db.SaveChanges();
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