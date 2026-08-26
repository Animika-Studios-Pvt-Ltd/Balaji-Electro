using System.Data;
using System.Linq;
using System.Web.Mvc;
using AudioPlanet.Helpers;
using AudioPlanet.Models;

namespace AudioPlanet.Controllers
{
    [Authorize]
    public class AdminUserController : Controller
    {
        private readonly Audio _db = new Audio();

        //
        // GET: /AdminUser/

        public ViewResult Index()
        {
            return View(_db.AdminUsers.ToList());
        }

        //
        // GET: /AdminUser/CheckDuplicate/Title
        public JsonResult CheckDuplicate(string name, string initialName)
        {
            bool result = !_db.AdminUsers.Any(a => a.Name == name && a.Name != initialName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /AdminUser/Details/5

        public ViewResult Details(int id)
        {
            AdminUser adminuser = _db.AdminUsers.Find(id);
            return View(adminuser);
        }

        //
        // GET: /AdminUser/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminUser/Create

        [HttpPost]
        public ActionResult Create(AdminUser adminuser)
        {
            if (ModelState.IsValid)
            {
                adminuser.Password = Utilities.EncodePassword(adminuser.Password);
                _db.AdminUsers.Add(adminuser);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminuser);
        }

        //
        // GET: /AdminUser/Edit/5

        public ActionResult Edit(int id)
        {
            AdminUser adminuser = _db.AdminUsers.Find(id);
            return View(adminuser);
        }

        //
        // POST: /AdminUser/Edit/5

        [HttpPost]
        public ActionResult Edit(AdminUser adminuser)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(adminuser).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminuser);
        }

        //
        // GET: /AdminUser/Delete/5

        public ActionResult Delete(int id)
        {
            AdminUser adminuser = _db.AdminUsers.Find(id);
            return View(adminuser);
        }

        //
        // POST: /AdminUser/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminUser adminuser = _db.AdminUsers.Find(id);
            _db.AdminUsers.Remove(adminuser);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Suspend(int id)
        {
            AdminUser adminuser = _db.AdminUsers.Find(id);
            try
            {
                adminuser.IsActive = false;
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

        public ActionResult Active(int id)
        {
            AdminUser adminuser = _db.AdminUsers.Find(id);
            try
            {
                adminuser.IsActive = true;
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
    }
}