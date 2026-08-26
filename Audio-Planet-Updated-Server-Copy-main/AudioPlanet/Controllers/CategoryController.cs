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
    public class CategoryController : Controller
    {
        //
        // GET: /Category/

        public static List<Category> allCats = new List<Category>();

        private readonly Audio _db = new Audio();

        public ActionResult Index()
        {
            var categories = _db.Categories;
            return View(categories.ToList());
        }

        public ActionResult Create()
        {
            var pageGroup = PageGroup.Page.ToString();
            var pages = _db.Pages.Where(p => p.PageGroup.Equals(pageGroup) && p.IsParent == false);
            var categories = getAllCategories();
            ViewBag.CategoryID = new SelectList(categories, "ID", "CategoryName");
            ViewBag.Keywords = from p in _db.Categories select p.MetaKeyword;
            return View();
        }

        public List<Category> getAllCategories()
        {
            getCategory(0);
            return allCats;
        }

        public void getCategory(int categoryId)
        {
            List<Category> parentCategories = new List<Category>();
            parentCategories = _db.Categories.Where(p => p.ParentCategoryId == categoryId).ToList();
            foreach (var parentCategory in parentCategories)
            {
                parentCategory.CategoryName = System.String.Concat(Enumerable.Repeat("-", parentCategory.Depth)) + parentCategory.CategoryName;
                allCats.Add(parentCategory);
                getCategory(parentCategory.ID);
            }

        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == 0)
                {
                    category.Depth = 0;
                    category.FullPath = category.CategoryUrl;
                    category.CategoryUrl = category.CategoryName.Replace(" ", "-").ToLower();
                    _db.Categories.Add(category);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }

                

                var fullPath = _db.Categories.FirstOrDefault(p => p.ID == category.ParentCategoryId);

                //===========depth==========

                category.Depth = fullPath.Depth + 1;

                //===========depth==========
                category.CategoryUrl = category.CategoryName.Replace(" ", "-").ToLower();
                category.FullPath = string.Concat(fullPath.FullPath, "/", category.CategoryUrl);
                _db.Categories.Add(category);
                //_db.Entry(temp).Property("CategoryUrl").IsModified = true;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            var pageGroup = PageGroup.Page.ToString();
            var pages = _db.Pages.Where(p => p.PageGroup.Equals(pageGroup) && p.IsParent == false);
            var categories = _db.Categories;
            ViewBag.CategoryID = new SelectList(categories, "ID", "CategoryName");

            

            return View(category);
        }

        public ActionResult Edit(int id)
        {
            Category category = _db.Categories.Find(id);
            //ViewBag.PageID = new SelectList(_db.Pages, "ID", "PageCode", category.);
            //var categories = getAllCategories();
            //ViewBag.CategoryID = new SelectList(categories, "ID", "CategoryName");
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            //if (ModelState.IsValid)
            //{
            if (category.ParentCategoryId == 0)
            {
                category.FullPath = category.CategoryUrl;
                category.CategoryUrl = category.CategoryName.Replace(" ", "-").ToLower();
                _db.Entry(category).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            var fullPath = _db.Categories.FirstOrDefault(p => p.ID == category.ParentCategoryId);
            category.Depth = fullPath.Depth + 1;
            category.FullPath = string.Concat(fullPath.FullPath, "/", category.CategoryUrl);
            //var oldCategories = new Category() {};
            //var temp = _db.Categories.FirstOrDefault(p => p.ID == category.ID);
            //var oldCategory = _db.Categories.Where(c => c.FullPath.Contains(temp.CategoryUrl));
            //foreach (var oldCategoryName in oldCategory)
            //{
            //    oldCategoryName.FullPath.Replace(temp.CategoryUrl, category.CategoryUrl);
            //}
            //_db.Categories.Attach(oldCategories);
            //_db.Entry(oldCategories).Property(p => p.FullPath).IsModified = true;
            category.CategoryUrl = category.CategoryName.Replace(" ", "-").ToLower();

            _db.Entry(category).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
            //}
            //return View(category);
        }

        //public ActionResult test()
        //{
        //    var oldCategories = new Category();
        //    var temp = _db.Categories.FirstOrDefault(p => oldCategories.ID == category.ParentCategoryId);
        //    var oldCategory = _db.Categories.Where(c => oldCategories.FullPath.Contains(temp.CategoryName));
        //    foreach (var oldCategoryName in oldCategory)
        //    {
        //        oldCategoryName.FullPath.Replace(temp.CategoryName, category.CategoryName);
        //    }
        //    _db.Categories.Attach(oldCategories);
        //    _db.Entry(oldCategories).Property(p => p.FullPath).IsModified = true;
        //    _db.SaveChanges();
        //    return null;
        //}

        public ActionResult Active(int id)
        {
            Category category = _db.Categories.Find(id);
            try
            {
                category.IsActive = true;
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
            Category category = _db.Categories.Find(id);
            try
            {
                category.IsActive = false;
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
            Category category = _db.Categories.Find(id);
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _db.Categories.Find(id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
