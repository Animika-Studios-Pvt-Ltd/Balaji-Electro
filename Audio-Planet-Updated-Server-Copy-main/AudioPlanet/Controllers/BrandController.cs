using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AudioPlanet.Models;

namespace AudioPlanet.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        //
        // GET: /Brand/

        private readonly Audio _db = new Audio();

        public ActionResult Index()
        {
            var brands = _db.Brands;
            return View(brands);
        }

        public ActionResult Create()
        {
            ViewBag.Keywords = from p in _db.Brands select p.MetaKeyword;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            //if (ModelState.IsValid)
            //{
                brand.BrandUrl = brand.BrandName.Replace(" ", "-").ToLower();
                _db.Brands.Add(brand);
                _db.SaveChanges();
                return View("Index");
            //}
            //return View(brand);
        }

        public ActionResult Edit(int id)
        {
            Brand brand = _db.Brands.Find(id);
            return View(brand);
        }

        [HttpPost]
        public ActionResult Edit(Brand brand)
        {
            brand.BrandUrl = brand.BrandName.Replace(" ", "-").ToLower();
            _db.Entry(brand).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Active(int id)
        {
            Brand brand = _db.Brands.Find(id);
            try
            {
                brand.IsActive = true;
                _db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Suspend(int id)
        {
            Brand brand = _db.Brands.Find(id);
            try
            {
                brand.IsActive = false;
                _db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Brand brands = _db.Brands.Find(id);
            return View(brands);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = _db.Brands.Find(id);
            _db.Brands.Remove(brand);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
