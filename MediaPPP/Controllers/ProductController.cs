using MediaPPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediaPPP.Repositories;
using System.IO;

namespace MediaPPP.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string prodName, string desc)
        {
            IList<Product> productList = new List<Product>();
            ProdutRepo repo = new ProdutRepo();
            CategoryRepo categoryRepo = new CategoryRepo();
            productList=repo.Filter(prodName,desc);
             
            ViewBag.CategoriesList= categoryRepo.GetAll();
            return View(productList);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }
            ProdutRepo repo = new ProdutRepo();
            Product product=repo.GetById(id);
            if(product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [Authorize(Roles ="Admin, Manager")]
        // GET: Product/Create
        public ActionResult Create()
        {
            
            CategoryRepo catRepo = new CategoryRepo();
            var categories = catRepo.GetAll();
            ViewBag.CategoriesList = categories;
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase imageFile)
        {
            var repo=new ProdutRepo();
            if (imageFile?.ContentLength > 0)
            {
                using(var stream = new MemoryStream())
                {
                    imageFile.InputStream.CopyTo(stream);
                    product.ProductImage = stream.ToArray();
                }
            }
            
            repo.Create(product);

            CategoryRepo catRepo=new CategoryRepo();
            var categories = catRepo.GetAll();
            ViewBag.CategoriesList = categories;
            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin,Manager")]
        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var repo=new ProdutRepo();  
            var product=repo.GetById(id);
            CategoryRepo catRepo = new CategoryRepo();
            var categories = catRepo.GetAll();
            ViewBag.CategoriesList = categories;
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var repo = new ProdutRepo();
            try
            {
                // TODO: Add update logic here
                repo.Update(product);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View();
            }
        }
        [Authorize(Roles ="Admin, Manager")]
        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var repo = new ProdutRepo();
            var prod=repo.GetById(id);
            return View(prod);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var repo = new ProdutRepo();
                repo.Delete(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public FileResult ShowImage(int id)
        {
            var repo = new ProdutRepo();
            var prod = repo.GetById(id);
            if (prod != null && prod.ProductImage != null)
                return File(prod.ProductImage, "image/jpeg", prod.ProductName + ".jpg");
            return null;
        }
    }
}
