using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AudioPlanet.Models;

namespace AudioPlanet.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly Audio _db = new Audio();

        //
        // GET: /Article/

        public ViewResult Index()
        {
            return View(_db.Articles.ToList());
        }

        //
        // GET: /Article/Details/5

        public ViewResult Details(int id)
        {
            Article article = _db.Articles.Find(id);
            return View(article);
        }

        //
        // GET: /Article/Create

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(_db.Pages.Where(p => p.PageGroup.Contains("Product")), "ID", "Title");
            return View();
        }

        //
        // POST: /Article/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.PostedTime = DateTime.Now;
                _db.Articles.Add(article);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(_db.Pages.Where(p => p.PageGroup.Contains("Product")), "ID", "Title");
            return View(article);
        }

        //
        // GET: /Article/Edit/5

        public ActionResult Edit(int id)
        {
            Article article = _db.Articles.Find(id);
            ViewBag.CategoryID = new SelectList(_db.Pages.Where(p => p.PageGroup.Contains("Product")), "ID", "Title",
                article.CategoryID);
            return View(article);
        }

        //
        // POST: /Article/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                article.PostedTime = Convert.ToDateTime(article.PostedTime);
                _db.Entry(article).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(_db.Pages.Where(p => p.PageGroup.Contains("Product")), "ID", "Title",
                article.CategoryID);
            return View(article);
        }

        public ActionResult ActiveStatus(int id, bool activeStatus)
        {
            Article article = _db.Articles.Find(id);
            try
            {
                article.IsActive = activeStatus;
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
            Article article = _db.Articles.Find(id);
            try
            {
                article.IsFeatured = featuredStatus;
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