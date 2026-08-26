using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AudioPlanet.Models;
using System.Collections.Generic;

namespace AudioPlanet.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly Audio _db = new Audio();
        public static List<Category> allCats = new List<Category>();

        //
        // GET: /Product/

        public ViewResult Index()
        {
            var products = _db.Products.Where(p => p.IsActive == true);
            return View(products.ToList());
        }

        //
        // GET: /Product/Details/5

        public ViewResult Details(int id)
        {
            Product product = _db.Products.Find(id);
            return View(product);
        }

        //
        // GET: /Product/Create

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

            var pageGroup = PageGroup.Product.ToString();
            var pages = _db.Pages.Where(p => p.PageGroup.Equals(pageGroup) && p.IsParent == false);
            ViewBag.PageID = new SelectList(pages, "ID", "Title");
            //var categories = _db.Categories.Where(p=>p.ID==0);
            var categories = getAllCategories();
            ViewBag.CategoryID = new SelectList(categories, "ID", "CategoryName");
            var brands = _db.Brands;
            ViewBag.BrandID = new SelectList(brands, "ID", "BrandName");
            ViewBag.Keywords = from p in _db.Products select p.MetaKeyword;
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var fullPath = _db.Categories.FirstOrDefault(p => p.ID == product.CategoryID);
                product.ProductFullUrl = string.Concat(fullPath.FullPath, "/", product.ProductUrl);
                product.ProductUrl = product.Name.Replace(" ", "-").ToLower();
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            var pageGroup = PageGroup.Product.ToString();
            var pages = _db.Pages.Where(p => p.PageGroup.Equals(pageGroup) && p.IsParent == false);
            ViewBag.PageID = new SelectList(pages, "ID", "Title");
            //var categories = _db.Categories.Where(p=>p.ID==0);
            var categories = getAllCategories();
            ViewBag.CategoryID = new SelectList(categories, "ID", "CategoryName");
            var brands = _db.Brands;
            ViewBag.BrandID = new SelectList(brands, "ID", "BrandName");
            return View(product);
        }

        //
        // GET: /Product/Edit/5
        //Sunil: TO-DO Require updation for dynamic brand & category selection
        public ActionResult Edit(int id)
        {
            Product product = _db.Products.Find(id);
            ViewBag.PageID = new SelectList(_db.Pages, "ID", "PageCode", product.PageID);
            return View(product);
        }

        //
        // POST: /Product/Edit/5
        //Sunil: TO-DO Require updation for dynamic brand & category selection
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var fullPath = _db.Categories.FirstOrDefault(p => p.ID == product.CategoryID);
                product.ProductFullUrl = string.Concat(fullPath.FullPath, "/", product.ProductUrl);
                product.ProductUrl = product.Name.Replace(" ", "-").ToLower();
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PageID = new SelectList(_db.Pages, "ID", "PageCode", product.PageID);
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id)
        {
            Product product = _db.Products.Find(id);
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _db.Products.Find(id);
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Active(int id)
        {
            Product product = _db.Products.Find(id);
            try
            {
                product.IsActive = true;
                _db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.lumos.in).";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Suspend(int id)
        {
            Product product = _db.Products.Find(id);
            try
            {
                product.IsActive = false;
                _db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.lumos.in).";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Upload(int id)
        {
            ViewBag.Product = _db.Products.Find(id);
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, FormCollection frm, int id)
        {
            try
            {
                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the fielname
                    string fileName = Path.GetFileName(file.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    if (fileName != null)
                    {
                        if (frm["mediaType"] != null)
                        {
                            var type = frm["mediaType"].ToString(CultureInfo.InvariantCulture);
                            var dir = new DirectoryInfo(Server.MapPath(string.Format("~/Content/Uploads/Product/{0}/{1}/", id, type)));
                            if (dir.Exists)
                            {
                                FileInfo[] files = dir.GetFiles();
                                foreach (FileInfo fileInfo in files)
                                {
                                    fileInfo.Delete();
                                }
                            }
                            else
                            {
                                dir.Create();
                            }

                            fileName = "Default" + Path.GetExtension(fileName);
                            string path = Path.Combine(dir.FullName, fileName);
                            file.SaveAs(path);
                        }
                    }
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.lumos.in).";
            }
            return RedirectToAction("Upload", new {id});
        }
    }
}