using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AudioPlanet.Models;

namespace AudioPlanet.Controllers
{
    public class ReviewController : Controller
    {
        private readonly Audio _db = new Audio();
        public static List<Category> allCats = new List<Category>();
        // GET: /Review/

        public ActionResult Index()
        {
            var review = _db.Reviews.ToList();
            return View(review);
        }

        public ActionResult Details(int id)
        {
            Review review = _db.Reviews.Find(id);
            return View(review);
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
                //string depth = System.String.Concat(Enumerable.Repeat("Hel", parentCategory.Depth));
                parentCategory.CategoryName = System.String.Concat(Enumerable.Repeat("-", parentCategory.Depth)) + parentCategory.CategoryName;
                allCats.Add(parentCategory);
                getCategory(parentCategory.ID);
            }
        }

        public ActionResult Create()
        {
            //var pageGroup = PageGroup..ToString();
            //var pages = _db.Pages.Where(p => p.PageGroup.Equals(pageGroup) && p.IsParent == false);
            //ViewBag.PageID = new SelectList(pages, "ID", "Title");
            //var categories = _db.Categories.Where(p=>p.ID==0);
            var categories = getAllCategories();
            ViewBag.CategoryID = new SelectList(categories, "ID", "CategoryName");
            List<SelectListItem> listItem = new List<SelectListItem>();
            var model = _db.Products.Where(p => p.IsActive == true);
            foreach (var item in model)
            {
                listItem.Add(new SelectListItem { Text = item.ID.ToString(), Value = item.Name });
            }
            listItem.Add(new SelectListItem { Text = "352", Value = "Others" });
            ViewBag.ProductID = new SelectList(listItem, "Text", "Value");
            return View();
        }

        //
        // POST: /Review/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.PostedTime = DateTime.Now;
                if (review.ProductName == null || review.ProductName == string.Empty)
                {
                    var productName = _db.Products.FirstOrDefault(p => p.ID == review.ProductID);
                    review.ProductName = productName.Name;
                }
                _db.Reviews.Add(review);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            var categories = getAllCategories();
            ViewBag.CategoryID = new SelectList(categories, "ID", "CategoryName");
            List<SelectListItem> listItem = new List<SelectListItem>();
            var model = _db.Products.Where(p => p.IsActive == true);
            foreach (var item in model)
            {
                listItem.Add(new SelectListItem { Text = item.ID.ToString(), Value = item.Name });
            }
            listItem.Add(new SelectListItem { Text = "352", Value = "Others" });
            ViewBag.ProductID = new SelectList(listItem, "Text", "Value");
            return View(review);
        }

        //
        // GET: /Article/Edit/5

        public ActionResult Edit(int id)
        {
            Review review = _db.Reviews.Find(id);
            var categories = getAllCategories();
            ViewBag.CategoryID = new SelectList(categories, "ID", "CategoryName", review.CategoryID);

            List<SelectListItem> listItem = new List<SelectListItem>();
            var model = _db.Products.Where(p => p.IsActive == true);
            foreach (var item in model)
            {
                listItem.Add(new SelectListItem { Text = item.ID.ToString(), Value = item.Name });
            }
            listItem.Add(new SelectListItem { Text = "352", Value = "Others" });
            ViewBag.ProductID = new SelectList(listItem, "Text", "Value", review.ProductID);
            return View(review);
        }

        //
        // POST: /Article/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Review review)
        {
            if (ModelState.IsValid)
            {
                review.PostedTime = Convert.ToDateTime(review.PostedTime);
                if (review.ProductName == null || review.ProductName == string.Empty)
                {
                    var productName = _db.Products.FirstOrDefault(p => p.ID == review.ProductID);
                    review.ProductName = productName.Name;
                }
                _db.Entry(review).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            var categories = getAllCategories();
            ViewBag.CategoryID = new SelectList(categories, "ID", "CategoryName", review.CategoryID);

            List<SelectListItem> listItem = new List<SelectListItem>();
            var model = _db.Products.Where(p => p.IsActive == true);
            foreach (var item in model)
            {
                listItem.Add(new SelectListItem { Text = item.ID.ToString(), Value = item.Name });
            }
            listItem.Add(new SelectListItem { Text = "352", Value = "Others" });
            ViewBag.ProductID = new SelectList(listItem, "Text", "Value", review.ProductID);
            return View(review);
        }

        public ActionResult ActiveStatus(int id, bool activeStatus)
        {
            Review review = _db.Reviews.Find(id);
            try
            {
                review.IsActive = activeStatus;
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

        public ActionResult FeaturedStatus(int id, bool featuredStatus)
        {
            Review review = _db.Reviews.Find(id);
            try
            {
                review.IsFeatured = featuredStatus;
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

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
