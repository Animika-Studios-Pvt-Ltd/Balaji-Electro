using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AudioPlanet.Models;

namespace AudioPlanet.Controllers
{
    [Authorize]
    public class PageController : Controller
    {
        private readonly Audio _db = new Audio();

        //GET: List all the Primary Pages
        public ViewResult Index()
        {
            IQueryable<Page> primaryPages = _db.Pages.Where(p => p.IsParent);
            return View(primaryPages);
        }

        //GET: List all the Secondary Pages of a specified Parent Page
        public ViewResult SubPages(int id)
        {
            return View(_db.Pages.Find(id).ChildPages);
        }

        //GET: /Page/Details/5
        public ViewResult Details(int id)
        {
            //Get history of selected page
            Page page = _db.Pages.Find(id);
            ViewBag.History = _db.PagesHistory.Where(p => p.PageCode == page.PageCode).OrderByDescending(o => o.CreatedAt);
            return View(page);
        }

        // GET: /Page/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(_db.Pages, "ID", "Title");
            ViewBag.Keywords = from p in _db.Pages select p.Keyword;
            return View();
        }

        //
        // GET: /AdminUser/CheckDuplicate/Title
        public JsonResult CheckDuplicate(string url, string initialUrl)
        {
            bool result =
                !_db.Pages.Any(
                    a =>
                        a.Url.Equals(url, StringComparison.CurrentCultureIgnoreCase) &&
                        !a.Url.Equals(initialUrl, StringComparison.CurrentCultureIgnoreCase));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: /Page/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Page page, FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    page.IsPublished = !string.IsNullOrEmpty(frm["BtnPublish"]);
                    page.IsActive = true;
                    page.IsItShowInMenu = false;
                    page.IsCmsPage = true;
                    page.SubTitle = string.IsNullOrEmpty(page.SubTitle) ? page.Title : page.SubTitle;
                    page.PageGroup = string.IsNullOrEmpty(page.PageGroup) ? "Page" : page.PageGroup;

                    _db.Entry(page).State = EntityState.Added;
                    _db.SaveChanges();

                    Page parentPage = _db.Pages.SingleOrDefault(p => p.ID == page.ParentId);
                    if (parentPage != null)
                    {
                        page.ParentPage.IsParent = true;
                        page.ParentPage = parentPage;
                    }
                    page.PageCode = string.Format("{0}{1}", page.Url.Replace("_", string.Empty).Replace(" ", string.Empty), Convert.ToString(page.ID));
                    _db.SaveChanges();
                    return RedirectToAction("Details", new { id = page.ID });
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("",
                                         "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).");
            }
            return View(page);
        }

        // GET: /Page/Edit/5
        public ActionResult Edit(int id)
        {
            Page page = _db.Pages.Find(id);
            ViewBag.ParentId = new SelectList(_db.Pages, "ID", "Title", page.ParentId);
            ViewBag.Order = new SelectList(_db.Pages, page);
            ViewBag.Keywords = from p in _db.Pages select p.Keyword;
            return View(page);
        }

        // POST: /Page/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Page page, FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    object requestedPgae = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["id"];
                    if (requestedPgae != null)
                    {
                        int requestedPageId = Convert.ToInt32(requestedPgae);
                        Page currentPage = _db.Pages.FirstOrDefault(p => p.ID == requestedPageId);
                        if (currentPage != null)
                        {
                            //Add updated page in hitory, if something is modified
                            var ph = new PageHistory
                                {
                                    ID = currentPage.ID,
                                    Content = currentPage.Content,
                                    CreatedAt = currentPage.CreatedAt,
                                    Description = currentPage.Description,
                                    IsActive = currentPage.IsActive,
                                    IsCmsPage = currentPage.IsCmsPage,
                                    IsItShowInMenu = currentPage.IsItShowInMenu,
                                    IsParent = currentPage.IsParent,
                                    IsPublished = currentPage.IsPublished,
                                    Keyword = currentPage.Keyword,
                                    Url = currentPage.Url,
                                    ParentId = currentPage.ParentId,
                                    Name = currentPage.Name,
                                    Order = currentPage.Order,
                                    PageCode = currentPage.PageCode,
                                    Title = currentPage.Title
                                };
                            _db.PagesHistory.Add(ph);
                            _db.SaveChanges();
                            _db.Entry(currentPage).State = EntityState.Detached;
                        }
                    }

                    //page.IsPublished = !string.IsNullOrEmpty(frm["BtnPublish"]);
                    _db.Entry(page).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Subpages", new { id = page.ParentId });
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("",
                                         "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).");
            }
            ViewBag.ParentId = new SelectList(_db.Pages, "ID", "Title", page.ParentId);
            ViewBag.Order = new SelectList(_db.Pages, page);
            ViewBag.Keywords = from p in _db.Pages select p.Keyword;
            return View(page);
        }

        // GET: /Page/Delete/5
        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return View(_db.Pages.Find(id));
        }

        // POST: /Page/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Page page = _db.Pages.Find(id);
            try
            {
                _db.Pages.Remove(page);
                _db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                return RedirectToAction("Delete", new RouteValueDictionary { { "id", id }, { "saveChangesError", true } });
            }
            return RedirectToAction("Subpages", new { id = page.ParentId });
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Publish(int id)
        {
            Page page = _db.Pages.Find(id);
            try
            {
                page.IsPublished = true;
                _db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return RedirectToAction("Subpages", new { id = page.ParentId });
        }

        public ActionResult UnPublish(int id)
        {
            Page page = _db.Pages.Find(id);
            try
            {
                page.IsPublished = false;
                _db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return RedirectToAction("Subpages", new { id = page.ParentId });
        }

        public ActionResult Active(int id)
        {
            Page page = _db.Pages.Find(id);
            try
            {
                page.IsActive = true;
                _db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return RedirectToAction("Subpages", new { id = page.ParentId });
        }

        public ActionResult Suspend(int id)
        {
            Page page = _db.Pages.Find(id);
            try
            {
                page.IsActive = false;
                _db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return RedirectToAction("Subpages", new { id = page.ParentId });
        }

        public ActionResult UpdateToHistory(int id)
        {
            PageHistory pageHistory = _db.PagesHistory.Find(id);
            Page pageToBeUpdated = _db.Pages.FirstOrDefault(p => p.PageCode == pageHistory.PageCode);

            try
            {
                if (pageToBeUpdated != null && pageHistory != null)
                {
                    pageToBeUpdated.Content = pageHistory.Content;
                    pageToBeUpdated.CreatedAt = pageHistory.CreatedAt;
                    pageToBeUpdated.Description = pageHistory.Description;
                    pageToBeUpdated.IsActive = pageHistory.IsActive;
                    pageToBeUpdated.IsCmsPage = pageHistory.IsCmsPage;
                    pageToBeUpdated.IsItShowInMenu = pageHistory.IsItShowInMenu;
                    pageToBeUpdated.IsParent = pageHistory.IsParent;
                    pageToBeUpdated.IsPublished = pageHistory.IsPublished;
                    pageToBeUpdated.Keyword = pageHistory.Keyword;
                    pageToBeUpdated.Url = pageHistory.Url;
                    pageToBeUpdated.ParentId = pageHistory.ParentId;
                    pageToBeUpdated.Name = pageHistory.Name;
                    pageToBeUpdated.Order = pageHistory.Order;
                    pageToBeUpdated.PageCode = pageHistory.PageCode;
                    pageToBeUpdated.Title = pageHistory.Title;

                    _db.Entry(pageToBeUpdated).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return RedirectToAction("Subpages", new { id = pageToBeUpdated.ParentId });
        }

        [HttpPost]
        public ActionResult DeleteHistory(int id)
        {
            PageHistory pageHistory = _db.PagesHistory.Find(id);
            Page pageToBeUpdated = _db.Pages.FirstOrDefault(p => p.PageCode == pageHistory.PageCode);
            _db.PagesHistory.Remove(pageHistory);
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = pageToBeUpdated.ID });
        }
    }
}