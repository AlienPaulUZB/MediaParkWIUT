using MediaPPP.Models;
using MediaPPP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaPPP.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            IList<Category> categories = new List<Category>();
            CategoryRepo categoryRepo=new CategoryRepo();
            categories=categoryRepo.GetAll();
            return View(categories);
        }


        [Authorize]
        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            CategoryRepo categoryRepo=new CategoryRepo();
            categoryRepo.Create(category);
            return RedirectToAction("Index");
        }

        // GET: Category/Edit/5
        [Authorize(Roles ="Admin, Manager")]
        public ActionResult Edit(int id)
        {
            var categoryRepo = new CategoryRepo();
            var category=categoryRepo.GetById(id);
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            var categoryRepo=new CategoryRepo();
            try
            {
                categoryRepo.Update(category);
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Category/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var repo=new CategoryRepo();
            var cat=repo.GetById(id);
            return View(cat);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                // TODO: Add delete logic here
                var repo=new CategoryRepo();
                repo.Delete(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View();
            }
        }
    }
}
