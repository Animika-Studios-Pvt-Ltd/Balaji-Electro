using System.Data;
using System.Linq;
using System.Web.Mvc;
using AudioPlanet.Models;

namespace AudioPlanet.Controllers
{ 
    [Authorize]
    public class TestimonialController : Controller
    {
        private readonly Audio _db = new Audio();

        //
        // GET: /Testimonial/

        public ViewResult Index()
        {
            return View(_db.Testimonials.ToList());
        }

        //
        // GET: /Testimonial/Details/5

        public ViewResult Details(int id)
        {
            Testimonial testimonial = _db.Testimonials.Find(id);
            return View(testimonial);
        }

        //
        // GET: /Testimonial/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Testimonial/Create

        [HttpPost]
        public ActionResult Create(Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _db.Testimonials.Add(testimonial);
                _db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(testimonial);
        }
        
        //
        // GET: /Testimonial/Edit/5
 
        public ActionResult Edit(int id)
        {
            Testimonial testimonial = _db.Testimonials.Find(id);
            return View(testimonial);
        }

        //
        // POST: /Testimonial/Edit/5

        [HttpPost]
        public ActionResult Edit(Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(testimonial).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testimonial);
        }

        //
        // GET: /Testimonial/Delete/5
 
        public ActionResult Delete(int id)
        {
            Testimonial testimonial = _db.Testimonials.Find(id);
            return View(testimonial);
        }

        //
        // POST: /Testimonial/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Testimonial testimonial = _db.Testimonials.Find(id);
            _db.Testimonials.Remove(testimonial);
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
            Testimonial testimonial = _db.Testimonials.Find(id);
            try
            {
                testimonial.IsActive = true;
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

            Testimonial testimonial = _db.Testimonials.Find(id);
            try
            {
                testimonial.IsActive = false;
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

        public ActionResult Archive(int id)
        {

            Testimonial testimonial = _db.Testimonials.Find(id);
            try
            {
                testimonial.IsArchived = true;
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

        public ActionResult Retrieve(int id)
        {
            Testimonial testimonial = _db.Testimonials.Find(id);
            try
            {
                testimonial.IsArchived = false;
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